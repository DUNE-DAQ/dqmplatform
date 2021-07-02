using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DuneDaqMonitoringPlatform.Models
{
    public class SamplingProfile
    {
        public ICollection<DataDisplay> DataDisplays { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(50)]
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [StringLength(50)]
        [Display(Name = "Description")]
        public string PlottingType { get; set; }

        [StringLength(200)]
        [Required]
        [Display(Name = "Technique")]
        public string Description { get; set; }

        [Display(Name = "Factor")]
        public float Factor { get; set; }
    }
}
