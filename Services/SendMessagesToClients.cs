using DuneDaqMonitoringPlatform.Data;
using DuneDaqMonitoringPlatform.Hubs;
using DuneDaqMonitoringPlatform.Models;
using DuneDaqMonitoringPlatform.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuneDaqMonitoringPlatform.Actions
{
    public class SendMessagesToClients
    {
        private readonly ChartDataMessenger chartDataMessenger;
        private readonly IConfiguration configuration;

        public SendMessagesToClients(IConfiguration configuration)
        {
            this.configuration = configuration;

            InputsMessenger messenger = InputsMessenger.Instance;
            IInputMessage m = (IInputMessage)messenger;
            m.OnIncoming += m_OnIncoming;

            chartDataMessenger = ChartDataMessenger.Instance;            
        }

        private async void m_OnIncoming(object sender, EventArgs e)
        {
            using(MonitoringDbContext monitoringDbContext = new MonitoringDbContext(configuration))
            { 
                try
                {
                    string[] input = sender.ToString().Split(',');
                    string dataId = input[0];

                    //Get all the DataDisplayDatas containing the data input id
                    //List<DataDisplayData> correspondingDataDisplayDatas21 = await monitoringDbContext.DataDisplayData.Include(ddd => ddd.DataDisplay).Include(ddd => ddd.Data).Include(dd => dd.DataDisplay.DataType).ToListAsync();
                    List<DataDisplayData> correspondingDataDisplayDatas = await monitoringDbContext.DataDisplayData.Include(ddd => ddd.DataDisplay).Include(ddd => ddd.Data).Include(dd => dd.DataDisplay.DataType).Where(ddd => ddd.Data.Id == Guid.Parse(dataId)).ToListAsync();
                    Console.WriteLine("Message incoming 1, \t round: " + PerformenceTimer.TimerVariable.executionRound.ToString() + " Time elapsed (ms): " + ((DateTime.Now.Ticks - PerformenceTimer.TimerVariable.executionTime) / 10000).ToString());

                    bool subscriber = false;

                    //Console.WriteLine("Incoming Id : " + dataId);


                    foreach (var correspondingDataDisplayData in correspondingDataDisplayDatas)
                    {
                        //List of clients subscribed to the sent data display
                        List<string> subscribedClients = new List<string>();
                        //Check if any of the client is subscribed to one of those displays, if subscribed gets the data and send
                        foreach (var ConnectedClient in ChartHub.ConnectedClients)
                        {
                            foreach (var DataDisplay in ConnectedClient.DataDisplayList)
                            {
                                if (DataDisplay == correspondingDataDisplayData.DataDisplay.Id)
                                {
                                    subscribedClients.Add(ConnectedClient.IdClient);
                                    break;
                                }
                            }
                        }

                        if (subscribedClients.Count() != 0)
                        {
                            Console.WriteLine("Sending to subsribers, \t round: " + PerformenceTimer.TimerVariable.executionRound.ToString() + " Time elapsed (ms): " + ((DateTime.Now.Ticks - PerformenceTimer.TimerVariable.executionTime) / 10000).ToString());

                            List<string> paths = new List<string>();
                            paths.Add((input[2]));
                            List<string> storages = new List<string>();
                            storages.Add((input[3]));
                            //Data Id, .DataDisplay.DataType.PlottingType
                            chartDataMessenger.ChartDataMessage(new ChartData { Paths = paths, IsInit = false, dataId = correspondingDataDisplayData.Data.Id, DataDisplay = correspondingDataDisplayData.DataDisplay, SubscribedClients = subscribedClients, dataStorages = storages });
                            subscriber = true;
                        }
                    }

                    if (!subscriber)
                    {
                        Console.WriteLine("No subscribers, \t round: " + PerformenceTimer.TimerVariable.executionRound.ToString() + " Time elapsed (ms): " + ((DateTime.Now.Ticks - PerformenceTimer.TimerVariable.executionTime) / 10000).ToString());
                        Console.WriteLine("");
                        PerformenceTimer.TimerVariable.executionFinished = true;
                    }
                }
                catch(Exception exception)
                {
                    Console.WriteLine(exception.ToString());

                }

            }
        }
    }
}
