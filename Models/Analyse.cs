using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DuneDaqMonitoringPlatform.Models
{
    public class Analyse
    {
        
        public ICollection<AnalysisParameter> AnalysisParameters { get; set; }
        public ICollection<DataDisplayAnalyse> DataDisplayAnalyses { get; set; }
        public AnalysisSource AnalysisSource { get; set; }
        public Data Data { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Display(Name = "Running")]
        public float Running { get; set; }
        
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(250)]
        [Display(Name = "Description")]
        public string Description { get; set; }

    }
}
