using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuneDaqMonitoringPlatform.Models
{
    public class ChartData
    {

        public List<string> Paths { get; set; }
        public List<string> WriteTimes { get; set; }
        public List<string> SubscribedClients { get; set; }
        public DataDisplay DataDisplay { get; set; }
        public Boolean IsInit { get; set; }
        public Guid dataId { get; set; }
        public List<string> dataStorages { get; set; }
        public int runNumber { get; set; }
    }
}
