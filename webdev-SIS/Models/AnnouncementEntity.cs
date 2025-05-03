using System.ComponentModel.DataAnnotations;

namespace webdev_SIS.Models
{
    public class AnnouncementEntity
    {
        [Key]
        public int id { get; set; }

        [Required]
        public required string Header { get; set; }

        [Required]
        public required string Details { get; set; }

        [Required]
        public DateTime? DatePosted { get; set; }
    }
}
