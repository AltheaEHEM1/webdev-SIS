using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webdev_SIS.Models
{
    public class UnenrollEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]  // Ensure GradeLevel is not null
        public string? GradeLevel { get; set; }
        [Required]  // First name cannot be null
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        [Required]  // Last name cannot be null
        public string? LastName { get; set; }
        public string? Suffix { get; set; }
        [EmailAddress]  // Email must be a valid email format
        public string? Email { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        public string? CivilStatus { get; set; }
        [Required]
        public string? Sex { get; set; }
        public string? Country { get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }
        [Range(0, 3)]  // Height range example
        public decimal Height { get; set; }
        [Range(0, 200)]  // Weight range example
        public decimal Weight { get; set; }
        public string? Religion { get; set; }
        public string? Disability { get; set; }
        [Phone]  // Valid phone number format
        public string? Phone_number { get; set; }
        [Phone]  // Valid phone number format
        public string? Landline_number { get; set; }
        [Phone]  // Valid phone number format
        public string? Emergency_landline_number { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactNumber { get; set; }
        public string? Relationship { get; set; }
        public string? HouseNo { get; set; }
        public string? Barangay { get; set; }
        public string? Street { get; set; }
        public string? Municipality { get; set; }
        public string? Province { get; set; }
        public string? ZipCode { get; set; }
        public string? PermanentHouseNo { get; set; }
        public string? PermanentBarangay { get; set; }
        public string? PermanentStreet { get; set; }
        public string? PermanentMunicipality { get; set; }
        public string? PermanentProvince { get; set; }
        public string? PermanentZipCode { get; set; }
        public string? ParentFirstName { get; set; }
        public string? ParentMiddleName { get; set; }
        public string? ParentLastName { get; set; }

        [Phone]  // Valid phone number format
        public string? ParentContactNo { get; set; }
        public string? ParentRelationship { get; set; }
        public string? GuardianFirstName { get; set; }
        public string? GuardianMiddleName { get; set; }
        public string? GuardianLastName { get; set; }

        [Phone]  // Valid phone number format
        public string? GuardianContactNo { get; set; }
        public string? GuardianRelationship { get; set; }
        public string? SchoolName { get; set; }
        public string? SchoolAddress { get; set; }
        public string? SchoolContact { get; set; }
        public string? SchoolLandline { get; set; }
        public string? SchoolType { get; set; }
        public int YearOfGraduation { get; set; }
        [Required]  // Ensure LRN is required
        public string? LRN { get; set; }
        [Range(0, 100)]  // Example GWA range
        public decimal GWA { get; set; }
        public string? SectionName { get; set; }
        [DataType(DataType.Date)]  // Enrolled Date format
        public DateTime DateEnrolled { get; set; } = DateTime.Now;
        [DataType(DataType.Date)]  // Unenrolled Date format
        public DateTime UnenrolledDate { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
        public string? ReasonDescription { get; set; }
    }
}
