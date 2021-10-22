using DuneDaqMonitoringPlatform.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DuneDaqMonitoringPlatform.Actions;
using Microsoft.Extensions.Configuration;
using DuneDaqMonitoringPlatform.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DuneDaqMonitoringPlatform.Hubs
{
    public class ChartHub : Hub
    {
        private readonly MonitoringDbContext monitoringDbContext;
        ChartDataMessenger chartDataMessenger;

        public ChartHub(IConfiguration configuration)
        {
            monitoringDbContext = new MonitoringDbContext(configuration);
            chartDataMessenger = ChartDataMessenger.Instance;
        }

        public async Task UpdateTime(string time, string formatedTime)
        {
            await Clients.Caller.SendAsync("ReceiveTimeUpdate", time, formatedTime);
        }
        public async Task saveView(string pannelName)
        {
            //create a new pannel from a display
            var connectedClient = ConnectedClients.Where(c => c.IdClient == Context.ConnectionId).FirstOrDefault();

            Pannel pannel = new Pannel();

            pannel.Id = Guid.NewGuid();
            pannel.Name = pannelName;
            monitoringDbContext.Add(pannel);
            await monitoringDbContext.SaveChangesAsync();

            AnalysisPannel analysisPannel;

            foreach (var dataDisplayId in connectedClient.DataDisplayList)
            {
                analysisPannel = new AnalysisPannel();
                analysisPannel.Pannel = monitoringDbContext.Pannel.Where(d => d.Id == pannel.Id).FirstOrDefault();
                analysisPannel.DataDisplay = monitoringDbContext.DataDisplay.Where(d => d.Id == dataDisplayId).FirstOrDefault();

                analysisPannel.Id = Guid.NewGuid();
                monitoringDbContext.Add(analysisPannel);
                await monitoringDbContext.SaveChangesAsync();
            }

        }

        public async void UpdateDisplayFromTime(List<string> dataDisplayIds, string time)
        {
            DateTime startTime = Convert.ToDateTime(time);

            List<DataPathDisplayGuid> dataPathsList = new List<DataPathDisplayGuid>();
            Guid dataDisplayGuid;

            //Gets All the datapaths to order them by times, allowing to makes updates one after the other
            foreach (string dataDisplayId in dataDisplayIds)
            {
                dataDisplayGuid = Guid.Parse(dataDisplayId);

                foreach (Models.Data data in monitoringDbContext.DataDisplayData.Where(ddd => ddd.DataDisplay.Id == dataDisplayGuid).Include(ddd => ddd.Data).Select(ddd => ddd.Data).ToList())
                {
                    dataPathsList.Add(new DataPathDisplayGuid { DataPaths = monitoringDbContext.DataDisplayData.Where(ddd => ddd.Data == data && ddd.DataDisplay.Id == dataDisplayGuid).Include(dp => dp.Data).Select(ddd => ddd.Data.DataPaths.Where(dp => dp.Path != "").OrderByDescending(dp => dp.WriteTime).ToList()).FirstOrDefault(), DisplayGuid = Guid.Parse(dataDisplayId) });
                }
            }


            //Get a list of ordered timestamp from the moment selected by the user
            List<DateTime> chartDatasOrderedTimes = dataPathsList.Select(dpl => dpl.DataPaths).SelectMany(l => l).Select(dp => UnixToDatetime(dp.WriteTime)).Where(dp => dp >= startTime).OrderBy(wt => wt).Distinct().ToList();

            
            //Send the datapath in order to the display
            foreach (DateTime chartDatasOrderedTime in chartDatasOrderedTimes)
            {
                string timeFormated = chartDatasOrderedTime.ToString("yyyy-MM-ddTHH:mm:ss");
                await UpdateTime(chartDatasOrderedTime.ToString(), timeFormated);

                foreach (string dataDisplayId in dataDisplayIds)
                {

                    dataDisplayGuid = Guid.Parse(dataDisplayId);
                    List<string> subscribedClients = new List<string>();
                    subscribedClients.Add(Context.ConnectionId);

                    foreach (List<DataPath> dataPaths in dataPathsList.Where(dpl => dpl.DisplayGuid == dataDisplayGuid).Select(dpl => dpl.DataPaths))
                    {
                        if(dataPaths.Select(dp => UnixToDatetime(dp.WriteTime)).Contains(chartDatasOrderedTime))
                        {
                            List<string> paths = dataPaths.Where(dp => UnixToDatetime(dp.WriteTime) <= chartDatasOrderedTime).Select(dp => dp.Path).ToList();
                            List<string> storages = dataPaths.Where(dp => UnixToDatetime(dp.WriteTime) <= chartDatasOrderedTime).Select(dp => dp.Storage).ToList();

                            DataDisplay dataDisplay = monitoringDbContext.DataDisplay.Where(dd => dd.Id == dataDisplayGuid).Include(dd => dd.DataType).FirstOrDefault();

                            chartDataMessenger.ChartDataMessage(new ChartData { Paths = paths, dataId = dataPaths.First().Data.Id, IsInit = false, DataDisplay = dataDisplay, SubscribedClients = subscribedClients, dataStorages = storages});
                        }                        
                    }
                    //Possibly implement thread interupt or pause here from the javascrip, Will neet to send a thread identifier to client and story list of active UpdateDisplayFromTime threads statically
                }

                

                //Interrupt the loop if client exited
                if (!ConnectedClients.Select(cc => cc.IdClient).Contains(Context.ConnectionId)){ break; }

                Thread.Sleep(1000);
            }
        }

        public async void GetDisplayAtTime(string dataDisplayId, string time)
        {

            DateTime startTime = Convert.ToDateTime(time);

            Guid dataDisplayGuid = Guid.Parse(dataDisplayId);

            List<string> subscribedClients = new List<string>();
            subscribedClients.Add(Context.ConnectionId);

            foreach (Models.Data data in monitoringDbContext.DataDisplayData.Where(ddd => ddd.DataDisplay.Id == dataDisplayGuid).Include(ddd => ddd.Data).Select(ddd => ddd.Data).ToList())
            {
                List<DataPath> dataPaths = monitoringDbContext.DataDisplayData.Where(ddd => ddd.Data == data && ddd.DataDisplay.Id == dataDisplayGuid).Select(ddd => ddd.Data.DataPaths.Where(dp => dp.Path != "").OrderByDescending(dp => dp.WriteTime).ToList()).FirstOrDefault();
                List<string> paths = dataPaths.Where(dp => roundToSecond(dp.WriteTime) <= startTime).Select(dp => dp.Path).ToList();
                List<string> storages = dataPaths.Where(dp => roundToSecond(dp.WriteTime) <= startTime).Select(dp => dp.Storage).ToList();

                DataDisplay dataDisplay = monitoringDbContext.DataDisplay.Where(dd => dd.Id == dataDisplayGuid).Include(dd => dd.DataType).FirstOrDefault();

                chartDataMessenger.ChartDataMessage(new ChartData { Paths = paths, dataId = data.Id, IsInit = true, DataDisplay = dataDisplay, SubscribedClients = subscribedClients, dataStorages = storages });
            }


            //update time in javascript
            string timeFormated = startTime.ToString("yyyy-MM-ddTHH:mm:ss");
            await UpdateTime(startTime.ToString(), timeFormated);

        }

        public async void GetDisplayList(string dataDisplayId)
        {

            Guid dataDisplayGuid = Guid.Parse(dataDisplayId);
             
            var connectedClient = ConnectedClients.Where(c => c.IdClient == Context.ConnectionId).FirstOrDefault();

            connectedClient.DataDisplayList.Add(dataDisplayGuid);

            List<string> subscribedClients = new List<string>();
            subscribedClients.Add(Context.ConnectionId);

            await UpdateTime("", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));

            foreach (Models.Data data in monitoringDbContext.DataDisplayData.Where(ddd => ddd.DataDisplay.Id == dataDisplayGuid).Include(ddd => ddd.Data).Select(ddd => ddd.Data).ToList())
            {
                List<DataPath> dataPaths = monitoringDbContext.DataDisplayData.Where(ddd => ddd.Data == data && ddd.DataDisplay.Id == dataDisplayGuid).Select(ddd => ddd.Data.DataPaths.Where(dp => dp.Path != "").OrderByDescending(dp => dp.WriteTime).ToList()).FirstOrDefault().ToList();
                List<string> paths = dataPaths.Select(dp => dp.Path).ToList();
                List<string> storages = dataPaths.Select(dp => dp.Storage).ToList();

                DataDisplay dataDisplay = monitoringDbContext.DataDisplay.Where(dd => dd.Id == dataDisplayGuid).Include(dd => dd.DataType).FirstOrDefault();

                chartDataMessenger.ChartDataMessage(new ChartData { Paths = paths, dataId = data.Id, IsInit = true, DataDisplay = dataDisplay, SubscribedClients = subscribedClients, dataStorages = storages });
            }
        }

        //Rounds down the time to second son compraison doesn't get wronged by miliseconds
        private DateTime roundToSecond(string time)
        {
            DateTime dateTime = UnixToDatetime(time);
            dateTime = new DateTime(
                dateTime.Ticks - (dateTime.Ticks % TimeSpan.TicksPerSecond),
                dateTime.Kind
                );
            return dateTime;
        }

        //Those two overrides allows to maintain a list of connected clients
        public override Task OnConnectedAsync()
        {
            ConnectedClients.Add(new ConnectedClient(Context.ConnectionId));
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            Console.WriteLine("Client exited");
            ConnectedClients.Remove(ConnectedClients.Where(c => c.IdClient == Context.ConnectionId).FirstOrDefault());
            return base.OnDisconnectedAsync(exception);
        }

        //Contains a list of connected users with all their subscribed data displays
        public static List<ConnectedClient> ConnectedClients = new List<ConnectedClient>();


        public DateTime UnixToDatetime(string time)
        {
            //Convert the microsecond to second
            long timeLong = long.Parse(Regex.Replace(time, "[^0-9]", "").Substring(0, 10));

            return DateTimeOffset.FromUnixTimeSeconds(timeLong).DateTime;
        }
    }
}
