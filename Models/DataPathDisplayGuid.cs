using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DuneDaqMonitoringPlatform.Models
{
    public class DataPathDisplayGuid
    {
        public List<DataPath> DataPaths { get; set; }

        public Guid DisplayGuid { get; set; }
    }
}
