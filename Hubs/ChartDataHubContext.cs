using DuneDaqMonitoringPlatform.Actions;
using DuneDaqMonitoringPlatform.Models;
using DuneDaqMonitoringPlatform.Services;
using DuneDaqMonitoringPlatform.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DuneDaqMonitoringPlatform.Hubs
{
    public class ChartDataHubContext
    {

        private readonly IHubContext<ChartHub> _hubContext;


        public ChartDataHubContext(IHubContext<ChartHub> hubContext)
        {

            ChartDataMessenger chartDataMessenger = ChartDataMessenger.Instance;
            IChartDataMessage m = (IChartDataMessage)chartDataMessenger;
            m.OnIncoming += m_OnIncomingAsync;

            _hubContext = hubContext;
        }

        async void DataSender(ChartUpdate chartUpdate, string functionName, List<string> subscribedClients)
        {
            await _hubContext.Clients.Clients(subscribedClients).SendAsync(functionName, chartUpdate);

            Console.WriteLine("Data sent to clients, \t round: " + PerformenceTimer.TimerVariable.executionRound.ToString() + " Time elapsed (ms): " + ((DateTime.Now.Ticks - PerformenceTimer.TimerVariable.executionTime) / 10000).ToString());
            Console.WriteLine("");
            PerformenceTimer.TimerVariable.executionFinished = true;

        }

        public async void m_OnIncomingAsync(object sender, EventArgs e)
        {

            ChartData chartData = (ChartData)sender;

            int fileSerieLengthX = 0;
            int fileSerieLengthY = 0;
            int pathNumber = 0;
            bool lengthReachedX = false;
            bool lengthReachedY = false;

            //loops until breaked or all paths displayed allows to search files until the right plot length is obtained
            foreach (string path in chartData.Paths)
            {
                if (path != "")
                {
                    string pathConverted;
                    //Convert the path to UNIX system if RELEASE
                    #if DEBUG
                        pathConverted = pathConverterWin(path);
                    #else
                        pathConverted = pathConverterUNIX(path);
                    #endif

                    try
                    {
                        Console.WriteLine("Start Reading data, \t round: " + PerformenceTimer.TimerVariable.executionRound.ToString() + " Time elapsed (ms): " + ((DateTime.Now.Ticks - PerformenceTimer.TimerVariable.executionTime) / 10000).ToString());

                        List<int[]> dataArray = new List<int[]>();
                        List<string> labelsArray = new List<string>();
                        DataBSON dataBSON = new DataBSON();
                        //Chooses the reading function according to the storage type
                        switch (chartData.dataStorages[pathNumber])
                        {
                            case "StructuredFile":
                                dataArray = await DataStructuredFileReader(pathConverted);
                                break;
                            case "BSON":
                                dataBSON = await DataBSONReader(pathConverted);
                                dataArray = dataBSON.Datas;
                                labelsArray = dataBSON.Labels;
                                break;
                            default:
                                dataArray = await DataStructuredFileReader(pathConverted);
                                break;
                        }

                        ChartUpdate chartUpdate = new ChartUpdate { ChartName = "chartPlaceholder" + chartData.DataDisplay.Id, DataId = chartData.dataId.ToString(), ChartLabels = labelsArray.ToArray() , ChartType = chartData.DataDisplay.DataType.PlottingType, IsInit = chartData.IsInit, DataDisplayName = chartData.DataDisplay.Name, ChartLengthX = chartData.DataDisplay.PlotLengthX, ChartLengthY = chartData.DataDisplay.PlotLengthY, ChartData = dataArray.ToArray() };

                        Console.WriteLine("Finish reading data, \t round: " + PerformenceTimer.TimerVariable.executionRound.ToString() + " Time elapsed (ms): " + ((DateTime.Now.Ticks - PerformenceTimer.TimerVariable.executionTime) / 10000).ToString());

                        DataSender(chartUpdate, "ReceivePlotUpdate", chartData.SubscribedClients);
                     
                        //Check if the data length in X requested has been reached
                        if (fileSerieLengthX <= chartData.DataDisplay.PlotLengthX && chartData.DataDisplay.PlotLengthX != 0)
                        {
                            fileSerieLengthX += dataArray[0].Count();
                        }
                        else { lengthReachedX = true; }
                        if (fileSerieLengthY <= chartData.DataDisplay.PlotLengthY && chartData.DataDisplay.PlotLengthY != 0)
                        {
                            fileSerieLengthY += dataArray.Count();
                        }
                        else { lengthReachedY = true; }

                        if (lengthReachedX && lengthReachedY) { break; }
                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.Message.ToString());
                        Console.WriteLine("Error reading " + path);
                    }

                }
                pathNumber++;
            }
        }

        private async Task<List<int[]>> DataStructuredFileReader(string pathConverted)
        {
            List<int[]> dataArray = new List<int[]>();

            string lines = await System.IO.File.ReadAllTextAsync(pathConverted);

            string[] linesToArray = lines.Split("\\n");

            for (int i = 2; i < linesToArray.Length - 1; i += 2)
            {
                int[] histogramsData = new int[linesToArray[i].Length];
                dataArray.Add(Array.ConvertAll(linesToArray[i].Split(' ').Where(n => n != "").ToArray(), int.Parse));
            }
            return dataArray;
        }

        private Task<DataBSON> DataBSONReader(string pathConverted)
        {
            return Task.Run(() =>
            {
                DataBSON dataBSON;

                byte[] bsonData = System.IO.File.ReadAllBytes(pathConverted);
                using (BsonReader reader = new BsonReader(new MemoryStream(bsonData)))
                {
                    JsonSerializer serializer = new JsonSerializer();

                    dataBSON = serializer.Deserialize<DataBSON>(reader);
                }
                return dataBSON;
            });            
        }

        private string pathConverterUNIX(string path)
        {
            path = path.Replace(@"\\cernbox-smb", "");
            path = path.Replace(@"\", @"/");
            path = path.Replace(@"home-s", @"user/s");

            return path;
        }

        private string pathConverterWin(string path)
        {
            if (!File.Exists(path))
            {
                path = @"\\cernbox-smb" + path;
                path = path.Replace(@"/", @"\");
                path = path.Replace(@"home-s", @"user\s");
            }
            return path;
        }
    }
}
