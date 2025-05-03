//using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations;

//namespace webdev_SIS.Models
//{
//    public class ProfileEntity
//    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int Id { get; set; }

//        [Required]
//        [StringLength(100)]
//        public string? Name { get; set; }

//        [StringLength(100)]
//        public string? MiddleName { get; set; }

//        [Required]
//        [StringLength(100)]
//        public string? LastName { get; set; }

//        [StringLength(255)]
//        public string? Bio { get; set; }

//        public DateTime? DateOfBirth { get; set; }

//        [Required]
//        [StringLength(255)]
//        [EmailAddress]
//        public string? Email { get; set; }

//        [Required]
//        [StringLength(11, MinimumLength = 11)]
//        [RegularExpression("^[0-9]{11}$", ErrorMessage = "Phone must be exactly 11 digits.")]
//        public string? Phone { get; set; }

//        [Required]
//        [StringLength(10)]
//        [RegularExpression("^[0-9]+$", ErrorMessage = "House number must contain only digits.")]
//        public string? HouseNo { get; set; }

//        [StringLength(255)]
//        public string? Street { get; set; }

//        [StringLength(255)]
//        public string? Barangay { get; set; }

//        [StringLength(255)]
//        public string? City { get; set; }

//        [StringLength(255)]
//        public string? Province { get; set; }

//        [Required]
//        [StringLength(4, MinimumLength = 4)]
//        [RegularExpression("^[0-9]{4}$", ErrorMessage = "Zip Code must be exactly 4 digits.")]
//        public string? ZipCode { get; set; }

//        [StringLength(255)]
//        public string? PhotoPath { get; set; }
//    }
//}
