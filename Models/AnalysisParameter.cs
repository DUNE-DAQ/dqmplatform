using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DuneDaqMonitoringPlatform.Models
{
    public class AnalysisParameter
    {
        public ICollection<AnalysisResult> AnalysisResults { get; set; }
        public Parameter Parameter { get; set; }
        public Analyse Analyse { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Display(Name = "Degree")]
        public float Degree { get; set; }

        [Display(Name = "Interval")]
        public int Interval { get; set; }

        [Display(Name = "Type")]
        public string Type { get; set; }
    }
}
