using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DuneDaqMonitoringPlatform.Models
{
    public class Parameter
    {
        public ICollection<AnalysisParameter> AnalysisParameters { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Factor")]
        public float Factor { get; set; }
    }
}
