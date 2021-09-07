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
        public int[][] ChartData { get; set; }
        public string[] ChartLabels { get; set; }
        public int ChartLengthX { get; set; }
        public int ChartLengthY { get; set; }
        public Boolean IsInit { get; set; }
        public string ChartType { get; set; }
    }
}
