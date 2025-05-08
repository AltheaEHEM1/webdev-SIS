using System.ComponentModel.DataAnnotations;

namespace webdev_SIS.Models
{
    public class SchoolYearEntity
    {
        [Key]
        public int SchoolYearID { get; set; }
        public string SchoolYear { get; set; }

    }
}
