using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DuneDaqMonitoringPlatform.Models
{
    public class ConnectedClient
    {
        public ConnectedClient(string idClient)
        {
            IdClient = idClient;
            DataDisplayList = new List<Guid>();
        }

        public string IdClient { get; set; }
        public List<Guid> DataDisplayList { get; set; }
    }
}
