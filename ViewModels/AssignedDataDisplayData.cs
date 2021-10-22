using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DuneDaqMonitoringPlatform.Models;

namespace DuneDaqMonitoringPlatform.ViewModels
{
    public class AssignedDataDisplayData
    {
        public Guid DataDisplayDataId { get; set; }
        public string DisplayName { get; set; }
        public bool Assigned { get; set; }
        public string DataSourceName { get; set; }
    }
}
