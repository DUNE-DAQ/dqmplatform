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

            int fileSerieLength = 0;
            int pathNumber = 0;
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

                        List<float[]> dataArray = new List<float[]>();
                        List<string> chartLabelsArray = new List<string>();
                        List<string> xLabelsArray = new List<string>();
                        List<string> yLabelsArray = new List<string>();
                        List<string> seriesLabelsArray = new List<string>();
                        DataBSON dataBSON = new DataBSON();
                        string metadata = "";
                        //Chooses the reading function according to the storage type
                        switch (chartData.dataStorages[pathNumber])
                        {
                            case "StructuredFile":
                                dataArray = await DataStructuredFileReader(pathConverted);
                                break;
                            case "BSON":
                                dataBSON = await DataBSONReader(pathConverted);
                                //dataBSON = DataBSONReader2(pathConverted);
                                dataArray = dataBSON.Datas;
                                chartLabelsArray = dataBSON.ChartLabels;
                                seriesLabelsArray = dataBSON.SeriesLabels;
                                xLabelsArray = dataBSON.LabelXs;
                                yLabelsArray = dataBSON.LabelYs;
                                metadata = dataBSON.Metadata;
                                break;
                            default:
                                dataArray = await DataStructuredFileReader(pathConverted);
                                break;
                        }

                        if(dataArray != null && chartLabelsArray != null && seriesLabelsArray != null && xLabelsArray != null && yLabelsArray != null)
                        {
                            ChartUpdate chartUpdate = new ChartUpdate { ChartName = "chartPlaceholder" + chartData.DataDisplay.Id, runNumber = chartData.runNumber.ToString() , DataId = chartData.dataId.ToString(), Metadata = metadata, YLabels = yLabelsArray.ToArray(), XLabels = xLabelsArray.ToArray(), SeriesLabels = seriesLabelsArray.ToArray(), ChartLabels = chartLabelsArray.ToArray(), ChartType = chartData.DataDisplay.DataType.Name, ChartPlottingType = chartData.DataDisplay.DataType.PlottingType, IsInit = chartData.IsInit, DataDisplayName = chartData.DataDisplay.Name, ChartLength = chartData.DataDisplay.PlotLength, ChartData = dataArray.ToArray(), ChartTime = chartData.WriteTimes.First() };

                            Console.WriteLine("Finish reading data, \t round: " + PerformenceTimer.TimerVariable.executionRound.ToString() + " Time elapsed (ms): " + ((DateTime.Now.Ticks - PerformenceTimer.TimerVariable.executionTime) / 10000).ToString());

                            DataSender(chartUpdate, "ReceivePlotUpdate", chartData.SubscribedClients);

                            //Check if the data length requested has been reached
                            fileSerieLength += dataArray[0].Count();
                            //if 0 set as size, takes only one file at the time
                            if (chartData.DataDisplay.PlotLength == 0) { break; }
                            //Else displays util size reached
                            else if (fileSerieLength >= chartData.DataDisplay.PlotLength) { break; }

                        }
                    
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

        private async Task<List<float[]>> DataStructuredFileReader(string pathConverted)
        {
            List<float[]> dataArray = new List<float[]>();

            string lines = await System.IO.File.ReadAllTextAsync(pathConverted);

            string[] linesToArray = lines.Split("\\n");

            for (int i = 2; i < linesToArray.Length - 1; i += 2)
            {
                int[] histogramsData = new int[linesToArray[i].Length];
                dataArray.Add(Array.ConvertAll(linesToArray[i].Split(' ').Where(n => n != "").ToArray(), float.Parse));
            }
            return dataArray;
        }

        private Task<DataBSON> DataBSONReader(string pathConverted)
        {
            return Task.Run(() =>
            {
                DataBSON dataBSON = new DataBSON();
                try
                {

                    byte[] bsonData = System.IO.File.ReadAllBytes(pathConverted);
                    using (BsonReader reader = new BsonReader(new MemoryStream(bsonData)))
                    {
                        JsonSerializer serializer = new JsonSerializer();

                        dataBSON = serializer.Deserialize<DataBSON>(reader);
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message.ToString());
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
