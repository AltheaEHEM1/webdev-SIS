using System.ComponentModel.DataAnnotations;

namespace webdev_SIS.Models
{
    public class StudentEntity
    {

        [Key]
        public int Id { get; set; }
        public string? GradeLevel { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Suffix { get; set; }
        public string? Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? CivilStatus { get; set; }
        public string? Sex { get; set; }
        public string? Country { get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public string? Religion { get; set; }
        public string? Disability { get; set; }
        public string? Phone_number { get; set; }
        public string? Landline_number { get; set; }
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
        public string? ParentContactNo { get; set; }
        public string? ParentRelationship { get; set; }
        public string? GuardianFirstName { get; set; }
        public string? GuardianMiddleName { get; set; }
        public string? GuardianLastName { get; set; }
        public string? GuardianContactNo { get; set; }
        public string? GuardianRelationship { get; set; }
        public string? SchoolName { get; set; }
        public string? SchoolAddress { get; set; }
        public string? SchoolContact { get; set; }
        public string? SchoolLandline { get; set; }
        public string? SchoolType { get; set; }
        public int YearOfGraduation { get; set; }
        public string? LRN { get; set; }
        public decimal GWA { get; set; }
        public string? SectionName { get; set; }
        public DateTime DateEnrolled { get; set; } = DateTime.Now;

    }
}
