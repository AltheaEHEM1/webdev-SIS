using System.ComponentModel.DataAnnotations;

namespace webdev_SIS.Models
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 12, ErrorMessage = "Password must be at least 12 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{12,}$",
    ErrorMessage = "Password must include uppercase, lowercase, number, and special character.")]
        public string? PasswordHash { get; set; }

        public string? Role { get; set; } // "Admin", "Faculty", or "Admission"
        public string? Subject { get; set; } // For Faculty
        public string? Position { get; set; } // For Faculty
        public string? Status { get; set; } // "Active" or "Inactive"
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;


    }
}
