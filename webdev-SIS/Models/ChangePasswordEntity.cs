using System.ComponentModel.DataAnnotations;

namespace webdev_SIS.Models
{
    public class ChangePasswordEntity
    {
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{12,}$",
    ErrorMessage = "Password must be at least 12 characters and include uppercase, lowercase, number, and symbol.")]
        public string PasswordHash { get; set; }

    }
}
