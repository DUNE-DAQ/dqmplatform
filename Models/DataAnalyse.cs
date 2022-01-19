using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DuneDaqMonitoringPlatform.Models
{
    public class DataAnalyse
    {
        public Analyse Analyse { get; set; }
        public DataDisplay dataDisplay { get; set; }
        //public Models.Data Data { get; set; }

        public string description { get; set; } 
        public int channel { get; set; } 

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
    }
}
