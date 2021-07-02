using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DuneDaqMonitoringPlatform.Models
{
    public class DataPath
    {

        public ICollection<AnalysisResult> AnalysisResults { get; set; }
        public Data Data { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Display(Name = "Write time")]
        [Required]
        public string WriteTime { get; set; }

        [StringLength(256)]
        [Display(Name = "Path to data")]
        public string Path { get; set; }

        [StringLength(30)]
        [Display(Name = "Storage type")]
        public string Storage { get; set; }
    }
}
