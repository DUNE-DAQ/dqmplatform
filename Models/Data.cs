using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DuneDaqMonitoringPlatform.Models
{
    public class Data
    {
        public ICollection<DataPath> DataPaths { get; set; }
        public ICollection<DataDisplayData> DataDisplayDatas { get; set; }
        public ICollection<Analyse> Analyses { get; set; }
        

        public DataSource DataSource { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Display(Name = "Rentention period")]
        [Required]
        public string RententionTime { get; set; }

        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; }
    }
}
