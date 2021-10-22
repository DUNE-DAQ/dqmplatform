using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuneDaqMonitoringPlatform.ViewModels
{
    public class DataBSON
    {
        public List<string> LabelXs { get; set; }
        public List<string> LabelYs { get; set; }
        public List<string> SeriesLabels { get; set; }
        public List<string> ChartLabels { get; set; }
        public List<int[]> Datas { get; set; }
    }
}
