using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DuneDaqMonitoringPlatform.Models
{
    public class AnalysisResult
    {
        public AnalysisParameter AnalysisParameter { get; set; }
        public DataPath DataPath { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Display(Name = "Decision")]
        public string Decision { get; set; }

        [Display(Name = "Confidence")]
        public float Confidence { get; set; }
    }
}
