using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DuneDaqMonitoringPlatform.Models
{
    public class DataSource
    {
        public ICollection<Data> Datas { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Source")]
        public string Source { get; set; }

        [StringLength(250)]
        [Display(Name = "Description")]
        public string Description { get; set; }

    }
}
