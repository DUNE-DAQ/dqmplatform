using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuneDaqMonitoringPlatform.ViewModels
{
    public class DataBSON
    {
        public List<string> LabelXs { get; set; }
        public List<string> Labels { get; set; }
        public List<int[]> Datas { get; set; }
    }
}
