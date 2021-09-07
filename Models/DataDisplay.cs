using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DuneDaqMonitoringPlatform.Models
{
    public class DataDisplay
    {
        public ICollection<AnalysisPannel> AnalysisPannels { get; set; }
        public ICollection<DataDisplayData> DataDisplayDatas { get; set; }
        public ICollection<DataDisplayAnalyse> DataDisplayAnalyses { get; set; }

        public DataType DataType { get; set; }

        public SamplingProfile SamplingProfile { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }


        [Display(Name = "Length of the plot in X")]
        public int PlotLengthX { get; set; }

        [Display(Name = "Length of the plot in Y")]
        public int PlotLengthY { get; set; }

        [Display(Name = "Name")]
        [Required]
        public string Name { get; set; }
    }
}
