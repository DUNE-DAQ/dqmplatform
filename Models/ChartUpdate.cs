using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuneDaqMonitoringPlatform.Models
{
    public class ChartUpdate
    {
        public string ChartName { get; set; }
        public string DataDisplayName { get; set; }
        public string DataId { get; set; }
        public float[][] ChartData { get; set; }
        public string[] SeriesLabels { get; set; }
        public string[] ChartLabels { get; set; }
        public string[] XLabels { get; set; }
        public string[] YLabels { get; set; }
        public int ChartLength { get; set; }
        public Boolean IsInit { get; set; }
        public string ChartType { get; set; }
        public string ChartPlottingType { get; set; }
        public string ChartTime { get; set; }
        public string runNumber { get; set; }
        public string Metadata { get; set; }
    }
}
