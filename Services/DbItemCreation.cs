using DuneDaqMonitoringPlatform.Data;
using DuneDaqMonitoringPlatform.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuneDaqMonitoringPlatform.Services
{
    public static class DbItemCreation
    {
        //Override if no GUID specified
        public static void CreateDataDisplay(MonitoringDbContext context, DataSource dataSource, Models.Data data, Pannel pannel, string plottignName, string plottignType, string samplingProfile)
        {
            CreateDataDisplay(Guid.NewGuid(), context, dataSource, data, pannel, plottignName, plottignType, samplingProfile);
        }

        public static void CreateDataDisplay(Guid guidDataDispaly, MonitoringDbContext context, DataSource dataSource, Models.Data data, Pannel pannel, string plottignName, string plottignType, string samplingProfile)
        {
            DataDisplay dataDisplay = new DataDisplay();
            dataDisplay.Id = guidDataDispaly;
            dataDisplay.DataType = context.DataType.Where(d => d.PlottingType == plottignType && d.Name == plottignName).First();
            dataDisplay.SamplingProfile = context.SamplingProfile.Where(d => d.Name == samplingProfile).First();
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

            context.Add(dataDisplayData);
            context.Add(dataDisplay);
            context.Add(analysisPannel);

            context.SaveChanges();
        }

        public static DataDisplay CreateDataDisplay(Guid guidDataDispaly, MonitoringDbContext context, DataSource dataSource, Models.Data data, string plottignName, string plottignType, string samplingProfile)
        {
            DataDisplay dataDisplay = new DataDisplay();
            dataDisplay.Id = guidDataDispaly;
            dataDisplay.DataType = context.DataType.Where(d => d.PlottingType == plottignType && d.Name == plottignName).First();
            dataDisplay.SamplingProfile = context.SamplingProfile.Where(d => d.Name == samplingProfile).First();
            dataDisplay.Name = dataSource.Source + " " + data.Name;
            dataDisplay.PlotLength = 0;

            //Create the intermediate table between datas and displays
            DataDisplayData dataDisplayData = new DataDisplayData();
            dataDisplayData.Id = Guid.NewGuid();
            dataDisplayData.Data = data;
            dataDisplayData.DataDisplay = dataDisplay;

            context.Add(dataDisplayData);
            context.Add(dataDisplay);

            context.SaveChanges();

            return dataDisplay;
        }

        public static void CreateDataAnalyse(MonitoringDbContext context, DataDisplay dataDisplay, Models.Data data, Analyse analyse, string description, int channel)
        {
            DataAnalyse dataAnalyse = new DataAnalyse();
            dataAnalyse.Id = Guid.NewGuid();
            //dataAnalyse.Data = context.Data.Where(d => d == data).First();
            dataAnalyse.dataDisplay = dataDisplay;
            dataAnalyse.Analyse = analyse;
            dataAnalyse.description = description;
            dataAnalyse.channel = channel;

            context.Add(dataAnalyse);

            context.SaveChanges();
        }

        public static Models.Data CreateData(MonitoringDbContext context, DataSource dataSource, string name)
        {
            Models.Data data = new Models.Data();
            data.Id = Guid.NewGuid();
            //dataAnalyse.Data = context.Data.Where(d => d == data).First();
            data.Name = name;
            data.DataSource = dataSource;
            data.RententionTime = "0";

            context.Add(data);

            context.SaveChanges();

            return data;
        }
    }
}
