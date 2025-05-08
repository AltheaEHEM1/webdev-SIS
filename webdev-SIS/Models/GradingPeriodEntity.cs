using System.ComponentModel.DataAnnotations;

namespace webdev_SIS.Models
{
    public class GradingPeriodEntity
    {
        [Key]
        public int GradingPeriodID { get; set; }

        public string PeriodName { get; set; }

        public string PeriodStatus { get; set; }

        public int SchoolYearID { get; set; }
    }
}
