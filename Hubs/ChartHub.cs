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
using DuneDaqMonitoringPlatform.Services;
using Microsoft.AspNetCore.Hosting;

namespace DuneDaqMonitoringPlatform.Hubs
{
    public class ChartHub : Hub
    {
        private readonly MonitoringDbContext monitoringDbContext;
        ChartDataMessenger chartDataMessenger;
        KafkaProducer kafkaProducer;

        public ChartHub(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            monitoringDbContext = new MonitoringDbContext(configuration);
            chartDataMessenger = ChartDataMessenger.Instance;
            kafkaProducer = new KafkaProducer(configuration, webHostEnvironment);
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
                            List<string> times = dataPaths.Where(dp => UnixToDatetime(dp.WriteTime) <= chartDatasOrderedTime).Select(dp => ChartHub.UnixToDatetime(dp.WriteTime).ToString()).ToList();
                            int run = dataPaths.Where(dp => UnixToDatetime(dp.WriteTime) <= chartDatasOrderedTime).Select(dp => dp.Run).FirstOrDefault();

                            /*
                            List<string> paths = (  
                                from dp in dataPaths
                                where UnixToDatetime(dp.WriteTime) <= chartDatasOrderedTime
                                select dp.Path
                                ).ToList();*/

                            DataDisplay dataDisplay = monitoringDbContext.DataDisplay.Where(dd => dd.Id == dataDisplayGuid).Include(dd => dd.DataType).FirstOrDefault();

                            chartDataMessenger.ChartDataMessage(new ChartData { Paths = paths, runNumber = run, dataId = dataPaths.First().Data.Id, IsInit = false, DataDisplay = dataDisplay, SubscribedClients = subscribedClients, dataStorages = storages, WriteTimes = times });
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
                List<string> times = dataPaths.Where(dp => roundToSecond(dp.WriteTime) <= startTime).Select(dp => ChartHub.UnixToDatetime(dp.WriteTime).ToString()).ToList();
                int run = dataPaths.Where(dp => roundToSecond(dp.WriteTime) <= startTime).Select(dp => dp.Run).FirstOrDefault();

                DataDisplay dataDisplay = monitoringDbContext.DataDisplay.Where(dd => dd.Id == dataDisplayGuid).Include(dd => dd.DataType).FirstOrDefault();

                chartDataMessenger.ChartDataMessage(new ChartData { Paths = paths, runNumber = run, dataId = data.Id, IsInit = true, DataDisplay = dataDisplay, SubscribedClients = subscribedClients, dataStorages = storages, WriteTimes = times });
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
                List<string> times = dataPaths.Select(dp => ChartHub.UnixToDatetime(dp.WriteTime).ToString()).ToList();
                int run = dataPaths.Select(dp => dp.Run).FirstOrDefault();


                DataDisplay dataDisplay = monitoringDbContext.DataDisplay.Where(dd => dd.Id == dataDisplayGuid).Include(dd => dd.DataType).FirstOrDefault();

                chartDataMessenger.ChartDataMessage(new ChartData { Paths = paths, runNumber = run, dataId = data.Id, IsInit = true, DataDisplay = dataDisplay, SubscribedClients = subscribedClients, dataStorages = storages, WriteTimes = times });
            }
        }

        //Hnadling the display of an analysis
        public async void GetAnalysis(string analysisId, string channelNumber, string idData, string curve)
        {
            Analyse analyse = monitoringDbContext.Analyse.Find(Guid.Parse(analysisId));
            Models.Data dataSource = monitoringDbContext.Data.Where(d => d.Id == Guid.Parse(idData.Substring(0, idData.Length - 2))).Include(d => d.DataSource).FirstOrDefault();
            Guid guidDataDisplay;
            DataDisplay dataDisplay;


            string dataName = (analyse.Name + "_" + idData + "_" + channelNumber).Replace(' ', '_');
            //search or create the quierried data (different from the data soruce, the data represents the processed source)
            Models.Data data = monitoringDbContext.Data.Where(d => d.Name == dataName).Include(d => d.DataSource).FirstOrDefault();
            if (data == null)
            {
                data = DbItemCreation.CreateData(monitoringDbContext, dataSource.DataSource, dataName);
            }

            //Creates the data display with analysis ID if doesn't exist
            //Check if analysis existing, if yes gets it, else if null creates it 
            //IEnumerable<DataAnalyse> dataAnalyse = monitoringDbContext.DataAnalyse.Include(da => da.Analyse).Where(da => da.Analyse == analyse).Where(da => da.channel == Int32.Parse(channelNumber)).Include(da => da.dataDisplay);
            //IEnumerable<DataDisplay> dataDisplays = monitoringDbContext.DataAnalyse.Include(da => da.Analyse).Where(da => da.Analyse == analyse).Where(da => da.channel == Int32.Parse(channelNumber)).Include(da => da.dataDisplay).Select(da => da.dataDisplay);
            List<DataDisplay> dataDisplays = monitoringDbContext.DataAnalyse.Include(da => da.Analyse).Where(da => da.Analyse == analyse).Where(da => da.channel == Int32.Parse(channelNumber)).Include(da => da.dataDisplay).Select(da => da.dataDisplay).ToList();
            //Among selected, takes only the analyssis which's display contains the same data
            //dataAnalyse = dataAnalyse.Where(da => da.dataDisplay.DataDisplayDatas)
            DataDisplayData dataDisplayData = monitoringDbContext.DataDisplayData.Include(ddd => ddd.Data).Include(ddd => ddd.DataDisplay).Where(ddd => dataDisplays.Contains(ddd.DataDisplay)).Where(ddd => ddd.Data ==data).FirstOrDefault();

            if (dataDisplayData == null)
            {
                guidDataDisplay = Guid.NewGuid();
                dataDisplay = DbItemCreation.CreateDataDisplay(guidDataDisplay, monitoringDbContext, data.DataSource, data, "markers", "standard", "Default");
                DbItemCreation.CreateDataAnalyse(monitoringDbContext, dataDisplay, data, analyse, curve, Int32.Parse(channelNumber));
            }
            else
            {
                dataDisplay = dataDisplayData.DataDisplay;
                guidDataDisplay = dataDisplay.Id;
            }
            //creates the div in the page
            await Clients.Caller.SendAsync("CreateDivFromGuid", guidDataDisplay.ToString());

            var connectedClient = ConnectedClients.Where(c => c.IdClient == Context.ConnectionId).FirstOrDefault();
            connectedClient.DataDisplayList.Add(guidDataDisplay);


            //Send the Kafka message
            await kafkaProducer.SendMessageAsync("TimeMean" + "," + Guid.Parse(idData.Substring(0, idData.Length - 2)) + "," + channelNumber + "," + dataName + "," + data.DataSource.Source + "," + "500");

            //Subscribes the client to updates

            //Use GetDisplayList function


            

        }
        /*
        public void CreateDataDisplayWithGuid(Guid dataDisplayGuid, DataSource dataSource, Models.Data data, Pannel pannel, string plottignName, string plottignType, string samplingProfile)
        {
            DataDisplay dataDisplay = new DataDisplay();
            dataDisplay.Id = dataDisplayGuid
            dataDisplay.DataType = _context.DataType.Where(d => d.PlottingType == plottignType && d.Name == plottignName).First();
            dataDisplay.SamplingProfile = _context.SamplingProfile.Where(d => d.Name == samplingProfile).First();
            dataDisplay.Name = dataSource.Source + " " + data.Name;
            dataDisplay.PlotLength = 0;

            //Create the intermediate table between datas and displays
            DataDisplayData dataDisplayData = new DataDisplayData();
            dataDisplayData.Id = Guid.NewGuid();
            dataDisplayData.Data = data;
            dataDisplayData.DataDisplay = dataDisplay;

            //Create the intermediate table between pannels and displays
            AnalysisPannel analysisPannel = new AnalysisPannel();
            analysisPannel.Id = Guid.NewGuid();
            analysisPannel.Pannel = pannel;
            analysisPannel.DataDisplay = dataDisplay;

            _context.Add(dataDisplayData);
            _context.Add(dataDisplay);
            _context.Add(analysisPannel);


            _context.SaveChanges();
        }*/

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


        public static DateTime UnixToDatetime(string time)
        {
            //Convert the microsecond to second
            long timeLong = long.Parse(Regex.Replace(time, "[^0-9]", "").Substring(0, 10));

            return DateTimeOffset.FromUnixTimeSeconds(timeLong).DateTime;
        }
    }
}
