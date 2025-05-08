using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using system_SIS.Services;
using webdev_SIS.DataLayer;
using webdev_SIS.Models;
using webdev_SIS.Services;

namespace webdev_SIS.Controllers
{
    [Authorize] // Require authentication for all actions
    public class AdminPortalController : BaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<AdminPortalController> _logger;
        private readonly EmailService _emailService;
        private readonly UserRepository _userRepo;

        public AdminPortalController(ApplicationDbContext db, ILogger<AdminPortalController> logger, system_SIS.Services.EmailService emailService, UserRepository userRepo)
        {
            _db = db;
            _logger = logger;
            _emailService = emailService;
            _userRepo = userRepo;
        }

        //HOME------------------------------------------------------------------------------------------------------------------
        public async Task<IActionResult> Index()
        {
            var announcements = await _db.Announcements
                .FromSqlRaw("EXEC GetAllAnnouncements")
                .ToListAsync();

            var sortedAnnouncements = announcements.OrderByDescending(a => a.DatePosted).ToList();

            return View("~/Views/AdminPortal/Index.cshtml", sortedAnnouncements);
        }



        //ADMISSION - Accessible by Admin and Admission roles
        [Authorize(Roles = "Admin,Admission")]
        public async Task<IActionResult> Admission()
        {
            ViewData["ActiveMenu"] = "Admission";
            var admissions = await _db.Admissions
                .FromSqlRaw("EXEC GetAllAdmissions")
                .ToListAsync();

            return View("~/Views/AdminPortal/Admission.cshtml", admissions);
        }



        [HttpGet("ReviewApp/{Id}")]
        [Authorize(Roles = "Admin,Admission")]
        public async Task<IActionResult> ReviewApp(int Id)
        {
            ViewData["ActiveMenu"] = "Admission";

            try
            {
                var admissions = await _db.Admissions
                    .FromSqlRaw("EXEC GetByIdAdmissions @Id={0}", Id)
                    .ToListAsync();

                if (admissions == null || admissions.Count == 0)
                {
                    return NotFound(new { success = false, message = "Application not found." });
                }

                return View("~/Views/AdminPortal/ReviewApp.cshtml", admissions);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching application: {Exception}", ex);
                return StatusCode(500, new { success = false, message = "An error occurred while fetching the application." });
            }
        }



        [HttpPost]
        [Authorize(Roles = "Admin,Admission")]
        public async Task<IActionResult> AcceptApplication(int id)
        {
            try
            {
                // Find the admission record by ID
                var admission = await _db.Admissions.FirstOrDefaultAsync(a => a.Id == id);
                if (admission == null)
                {
                    return NotFound(new { success = false, message = "Application not found." });
                }

                // Create a new enrollment record with the admission details
                var enrollment = new EnrollmentEntity
                {
                    GradeLevel = admission.GradeLevel,
                    FirstName = admission.FirstName,
                    MiddleName = admission.MiddleName,
                    LastName = admission.LastName,
                    Suffix = admission.Suffix,
                    Email = admission.Email,
                    DateOfBirth = admission.DateOfBirth,
                    CivilStatus = admission.CivilStatus,
                    Sex = admission.Sex,
                    Country = admission.Country,
                    Region = admission.Region,
                    City = admission.City,
                    Height = admission.Height,
                    Weight = admission.Weight,
                    Religion = admission.Religion,
                    Disability = admission.Disability,
                    Phone_number = admission.Phone_number,
                    Landline_number = admission.Landline_number,
                    Emergency_landline_number = admission.Emergency_landline_number,
                    ContactPerson = admission.ContactPerson,
                    ContactNumber = admission.ContactNumber,
                    Relationship = admission.Relationship,
                    HouseNo = admission.HouseNo,
                    Barangay = admission.Barangay,
                    Street = admission.Street,
                    Municipality = admission.Municipality,
                    Province = admission.Province,
                    ZipCode = admission.ZipCode,

                    // Permanent Address
                    PermanentHouseNo = admission.PermanentHouseNo,
                    PermanentBarangay = admission.PermanentBarangay,
                    PermanentStreet = admission.PermanentStreet,
                    PermanentMunicipality = admission.PermanentMunicipality,
                    PermanentProvince = admission.PermanentProvince,
                    PermanentZipCode = admission.PermanentZipCode,

                    // Parent Information
                    ParentFirstName = admission.ParentFirstName,
                    ParentMiddleName = admission.ParentMiddleName,
                    ParentLastName = admission.ParentLastName,
                    ParentContactNo = admission.ParentContactNo,
                    ParentRelationship = admission.ParentRelationship,

                    // Guardian Information
                    GuardianFirstName = admission.GuardianFirstName,
                    GuardianMiddleName = admission.GuardianMiddleName,
                    GuardianLastName = admission.GuardianLastName,
                    GuardianContactNo = admission.GuardianContactNo,
                    GuardianRelationship = admission.GuardianRelationship,

                    // School Information
                    SchoolName = admission.SchoolName,
                    SchoolAddress = admission.SchoolAddress,
                    SchoolContact = admission.SchoolContact,
                    SchoolType = admission.SchoolType,
                    YearOfGraduation = admission.YearOfGraduation,
                    LRN = admission.LRN,
                    GWA = admission.GWA,
                    DateEnrolled = DateTime.Now
                };

                // Add new enrollment
                _db.Enrollment.Add(enrollment);

                // Remove admission from the database (or soft delete)
                _db.Admissions.Remove(admission);

                // Save changes to the database
                await _db.SaveChangesAsync();

                // ✅ SEND EMAIL AFTER SUCCESSFUL ACCEPTANCE
                //_emailService.SendEmail(admission.Email,
                //                        "Application Accepted",
                //                        "Congratulations! Your application has been accepted. Proceed to enrollment.");

                return RedirectToAction("Enrollments"); // Redirect to Enrollment Page
            }
            catch (Exception ex)
            {
                _logger.LogError("Error accepting application for AdmissionId: {AdmissionId}. Exception: {Exception}", id, ex);
                return StatusCode(500, new { success = false, message = "An error occurred while processing the application." });
            }
        }






        //FOR THE ENROLLMENT ----------------------------------------------------------------------------------------------------------
       

        //ENROLLMENT 
        [Authorize(Roles = "Admin,Admission")]
        public async Task<IActionResult> Enrollments()
        {
            ViewData["ActiveMenu"] = "Enrollments";
            var enrollments = await _db.Enrollment.ToListAsync();
            return View(enrollments);
        }


        //Enrollment Details
        [Authorize(Roles = "Admin,Admission")]
        public async Task<IActionResult> EnrollmentDetails(int Id)
        {
            ViewData["ActiveMenu"] = "Enrollments";

            try
            {
                var enrollment = await _db.Enrollment
                    .FromSqlRaw("EXEC GetByIdEnrollment @Id={0}", Id)
                    .ToListAsync();

                if (enrollment == null || enrollment.Count == 0)
                {
                    return NotFound(new { success = false, message = "Application not found." });
                }

                return View("~/Views/AdminPortal/EnrollmentDetails.cshtml", enrollment);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching enrollment details for EnrollmentId: {EnrollmentId}. Exception: {Exception}", Id, ex);
                return StatusCode(500, new { success = false, message = "An error occurred while fetching the application." });
            }
        }







        //Student view --------------------------------------------------------------------------------------------------------------------
        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult Students()
        {
            ViewData["ActiveMenu"] = "Students";
            return View();
        }



        //Assigning a section to a student
        [HttpGet]
        [Authorize(Roles = "Admin,Faculty")]
        public JsonResult GetSections(string gradeLevel)
        {
            var sections = new Dictionary<string, List<string>>
            {
                { "Grade 7", new List<string> { "Acacia", "Molave", "Ipil", "Kamagong", "Yakal", "Narra", "Mahogany", "Bamboo", "Lauan", "Jacaranda" } },
                { "Grade 8", new List<string> { "Perseverance", "Integrity", "Patience", "Courage", "Loyalty", "Compassion", "Humility", "Resilience", "Sincerity", "Fortitude" } },
                { "Grade 9", new List<string> { "Einstein", "Curie", "Pascal", "Faraday", "Darwin", "Bohr", "Edison", "Hawking", "Tesla", "Galileo" } },
                { "Grade 10", new List<string> { "Rizal", "Bonifacio", "Del Pilar", "Mabini", "Jacinto", "Luna", "Aguinaldo", "Silang", "Gomez", "Burgos" } }
            };

            if (sections.TryGetValue(gradeLevel, out var sectionList))
            {
                return Json(sectionList);
            }

            return Json(new List<string>());  // Return an empty list if the grade level is not found
        }



        [HttpPost]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> AssignSection([FromBody] StudentEntity request)
        {
            try
            {
                _logger.LogWarning("No enrollment record found for Student ID: {StudentId}", request.Id);

                var studentEnrollment = await _db.Enrollment.FindAsync(request.Id);

                if (studentEnrollment == null)
                {
                    _logger.LogWarning("No enrollment record found for Student ID: {StudentId}", request.Id);
                    return NotFound(new { success = false, message = "Enrollment record not found." });
                }

                var student = new StudentEntity
                {
                    GradeLevel = studentEnrollment.GradeLevel,
                    FirstName = studentEnrollment.FirstName,
                    MiddleName = studentEnrollment.MiddleName,
                    LastName = studentEnrollment.LastName,
                    Suffix = studentEnrollment.Suffix,
                    Email = studentEnrollment.Email,
                    DateOfBirth = studentEnrollment.DateOfBirth,
                    CivilStatus = studentEnrollment.CivilStatus,
                    Sex = studentEnrollment.Sex,
                    Country = studentEnrollment.Country,
                    Region = studentEnrollment.Region,
                    City = studentEnrollment.City,
                    Height = studentEnrollment.Height,
                    Weight = studentEnrollment.Weight,
                    Religion = studentEnrollment.Religion,
                    Disability = studentEnrollment.Disability,
                    Phone_number = studentEnrollment.Phone_number,
                    Landline_number = studentEnrollment.Landline_number,
                    Emergency_landline_number = studentEnrollment.Emergency_landline_number,
                    ContactPerson = studentEnrollment.ContactPerson,
                    ContactNumber = studentEnrollment.ContactNumber,
                    Relationship = studentEnrollment.Relationship,
                    HouseNo = studentEnrollment.HouseNo,
                    Barangay = studentEnrollment.Barangay,
                    Street = studentEnrollment.Street,
                    Municipality = studentEnrollment.Municipality,
                    Province = studentEnrollment.Province,
                    ZipCode = studentEnrollment.ZipCode,
                    PermanentHouseNo = studentEnrollment.PermanentHouseNo,
                    PermanentBarangay = studentEnrollment.PermanentBarangay,
                    PermanentStreet = studentEnrollment.PermanentStreet,
                    PermanentMunicipality = studentEnrollment.PermanentMunicipality,
                    PermanentProvince = studentEnrollment.PermanentProvince,
                    PermanentZipCode = studentEnrollment.PermanentZipCode,
                    ParentFirstName = studentEnrollment.ParentFirstName,
                    ParentMiddleName = studentEnrollment.ParentMiddleName,
                    ParentLastName = studentEnrollment.ParentLastName,
                    ParentContactNo = studentEnrollment.ParentContactNo,
                    ParentRelationship = studentEnrollment.ParentRelationship,
                    GuardianFirstName = studentEnrollment.GuardianFirstName,
                    GuardianMiddleName = studentEnrollment.GuardianMiddleName,
                    GuardianLastName = studentEnrollment.GuardianLastName,
                    GuardianContactNo = studentEnrollment.GuardianContactNo,
                    GuardianRelationship = studentEnrollment.GuardianRelationship,
                    SchoolName = studentEnrollment.SchoolName,
                    SchoolAddress = studentEnrollment.SchoolAddress,
                    SchoolContact = studentEnrollment.SchoolContact,
                    SchoolType = studentEnrollment.SchoolType,
                    YearOfGraduation = studentEnrollment.YearOfGraduation,
                    LRN = studentEnrollment.LRN,
                    GWA = studentEnrollment.GWA,
                    DateEnrolled = studentEnrollment.DateEnrolled,
                    SectionName = request.SectionName // ✅ Save the selected section
                };

                try
                {
                    _db.Students.Add(student);
                    _db.Enrollment.Remove(studentEnrollment);
                    await _db.SaveChangesAsync();
                }
                catch (Exception dbEx)
                {
                    _logger.LogError(dbEx, "Database error during SaveChangesAsync.");
                    return StatusCode(500, new { success = false, message = dbEx.Message });
                }

                return Json(new { success = true, section = student.SectionName });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error assigning section and moving student.");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }



        //studentlist
        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult StudentList(string section)
        {
            ViewData["ActiveMenu"] = "Students";

            // Store the section in ViewBag for the view
            ViewBag.SectionName = section;

            // Dictionary to map sections to Grade Levels
            var sections = new Dictionary<string, List<string>>
                {
                    { "Grade 7", new List<string> { "Acacia", "Molave", "Ipil", "Kamagong", "Yakal", "Narra", "Mahogany", "Bamboo", "Lauan", "Jacaranda" } },
                    { "Grade 8", new List<string> { "Perseverance", "Integrity", "Patience", "Courage", "Loyalty", "Compassion", "Humility", "Resilience", "Sincerity", "Fortitude" } },
                    { "Grade 9", new List<string> { "Einstein", "Curie", "Pascal", "Faraday", "Darwin", "Bohr", "Edison", "Hawking", "Tesla", "Galileo" } },
                    { "Grade 10", new List<string> { "Rizal", "Bonifacio", "Del Pilar", "Mabini", "Jacinto", "Luna", "Aguinaldo", "Silang", "Gomez", "Burgos" } }
                };

            // Find the GradeLevel based on the section name
            string gradeLevel = sections
                .FirstOrDefault(g => g.Value.Contains(section)).Key;

            ViewBag.GradeLevel = gradeLevel; // Will be null if section not found

            // Fetch students from database based on the section
            var students = _db.Students.Where(s => s.SectionName == section).ToList();

            return View(students);
        }









        //Student details and update function ------------------------------------------------------------------------------------------------------------
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> StudentDetail(int id)
        {
            ViewData["ActiveMenu"] = "Students";

            try
            {
                var students = await _db.Students
                    .FromSqlRaw("EXEC GetSingleStudentByGrade @Id={0}", id)
                    .ToListAsync();

                if (students == null || students.Count == 0)
                {
                    return NotFound(new { success = false, message = "Application not found." });
                }

                return View("~/Views/AdminPortal/StudentDetail.cshtml", students);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching student details for StudentId: {StudentId}. Exception: {Exception}", id, ex);
                return StatusCode(500, new { success = false, message = "An error occurred while fetching the student details." });
            }
        }



        // GET: Student details
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var students = await _db.Students
                    .FromSqlRaw("EXEC GetSingleStudentByGrade @Id={0}", id)
                    .ToListAsync();

                if (students == null || students.Count == 0)
                {
                    return NotFound(new { success = false, message = "Student not found." });
                }

                return View("~/Views/AdminPortal/StudentDetail.cshtml", students);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching student details for StudentId: {StudentId}. Exception: {Exception}", id, ex);
                return StatusCode(500, new { success = false, message = "An error occurred while fetching the student details." });
            }
        }



        // POST: Update Personal Information
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> UpdatePersonalInfo(StudentEntity model)
        {
            try
            {
                // Create parameters for stored procedure
                var parameters = new[]
                {
                    new SqlParameter("@Id", model.Id),
                    new SqlParameter("@FirstName", model.FirstName ?? (object)DBNull.Value),
                    new SqlParameter("@MiddleName", model.MiddleName ?? (object)DBNull.Value),
                    new SqlParameter("@LastName", model.LastName ?? (object)DBNull.Value),
                    new SqlParameter("@Suffix", model.Suffix ?? (object)DBNull.Value),
                    new SqlParameter("@Email", model.Email ?? (object)DBNull.Value),
                    new SqlParameter("@DateOfBirth", model.DateOfBirth),
                    new SqlParameter("@CivilStatus", model.CivilStatus ?? (object)DBNull.Value),
                    new SqlParameter("@Sex", model.Sex ?? (object)DBNull.Value),
                    new SqlParameter("@Country", model.Country ?? (object)DBNull.Value),
                    new SqlParameter("@Region", model.Region ?? (object)DBNull.Value),
                    new SqlParameter("@City", model.City ?? (object)DBNull.Value),
                    new SqlParameter("@Height", model.Height),
                    new SqlParameter("@Weight", model.Weight),
                    new SqlParameter("@Religion", model.Religion ?? (object)DBNull.Value),
                    new SqlParameter("@Disability", model.Disability ?? (object)DBNull.Value)
                };

                // Execute stored procedure
                await _db.Database.ExecuteSqlRawAsync(
                    "EXEC UpdateStudentPersonalInfo @Id, @FirstName, @MiddleName, @LastName, @Suffix, @Email, @DateOfBirth, @CivilStatus, @Sex, @Country, @Region, @City, @Height, @Weight, @Religion, @Disability",
                    parameters);

                return Json(new { success = true, message = "Personal information updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating personal information for StudentId: {StudentId}. Exception: {Exception}", model.Id, ex);
                return Json(new { success = false, message = "Error updating personal information: " + ex.Message });
            }
        }



        // POST: Update Contact Information
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> UpdateContact(StudentEntity model)
        {
            try
            {
                // Create parameters for stored procedure
                var parameters = new[]
                {
                    new SqlParameter("@Id", model.Id),
                    new SqlParameter("@Landline_number", model.Landline_number ?? (object)DBNull.Value),
                    new SqlParameter("@Phone_number", model.Phone_number ?? (object)DBNull.Value),
                    new SqlParameter("@Emergency_landline_number", model.Emergency_landline_number ?? (object)DBNull.Value),
                    new SqlParameter("@ContactPerson", model.ContactPerson ?? (object)DBNull.Value),
                    new SqlParameter("@ContactNumber", model.ContactNumber ?? (object)DBNull.Value),
                    new SqlParameter("@Relationship", model.Relationship ?? (object)DBNull.Value)
                };

                // Execute stored procedure
                await _db.Database.ExecuteSqlRawAsync(
                    "EXEC UpdateStudentContact @Id, @Landline_number, @Phone_number, @Emergency_landline_number, @ContactPerson, @ContactNumber, @Relationship",
                    parameters);

                return Json(new { success = true, message = "Contact information updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating contact information for StudentId: {StudentId}. Exception: {Exception}", model.Id, ex);
                return Json(new { success = false, message = "Error updating contact information: " + ex.Message });
            }
        }



        // POST: Update Address Information
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> UpdateAddress(StudentEntity model)
        {
            try
            {
                // Create parameters for stored procedure
                var parameters = new[]
                {
                    new SqlParameter("@Id", model.Id),
                    new SqlParameter("@HouseNo", model.HouseNo ?? (object)DBNull.Value),
                    new SqlParameter("@Barangay", model.Barangay ?? (object)DBNull.Value),
                    new SqlParameter("@Street", model.Street ?? (object)DBNull.Value),
                    new SqlParameter("@Municipality", model.Municipality ?? (object)DBNull.Value),
                    new SqlParameter("@Province", model.Province ?? (object)DBNull.Value),
                    new SqlParameter("@ZipCode", model.ZipCode ?? (object)DBNull.Value),
                    new SqlParameter("@PermanentHouseNo", model.PermanentHouseNo ?? (object)DBNull.Value),
                    new SqlParameter("@PermanentBarangay", model.PermanentBarangay ?? (object)DBNull.Value),
                    new SqlParameter("@PermanentStreet", model.PermanentStreet ?? (object)DBNull.Value),
                    new SqlParameter("@PermanentMunicipality", model.PermanentMunicipality ?? (object)DBNull.Value),
                    new SqlParameter("@PermanentProvince", model.PermanentProvince ?? (object)DBNull.Value),
                    new SqlParameter("@PermanentZipCode", model.PermanentZipCode ?? (object)DBNull.Value)
                };

                // Execute stored procedure
                await _db.Database.ExecuteSqlRawAsync(
                    "EXEC UpdateStudentAddress @Id, @HouseNo, @Barangay, @Street, @Municipality, @Province, @ZipCode, @PermanentHouseNo, @PermanentBarangay, @PermanentStreet, @PermanentMunicipality, @PermanentProvince, @PermanentZipCode",
                    parameters);

                return Json(new { success = true, message = "Address information updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating address information for StudentId: {StudentId}. Exception: {Exception}", model.Id, ex);
                return Json(new { success = false, message = "Error updating address information: " + ex.Message });
            }
        }



        // POST: Update Family Information
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> UpdateFamily(StudentEntity model)
        {
            try
            {
                // Create parameters for stored procedure
                var parameters = new[]
                {
                    new SqlParameter("@Id", model.Id),
                    new SqlParameter("@ParentFirstName", model.ParentFirstName ?? (object)DBNull.Value),
                    new SqlParameter("@ParentMiddleName", model.ParentMiddleName ?? (object)DBNull.Value),
                    new SqlParameter("@ParentLastName", model.ParentLastName ?? (object)DBNull.Value),
                    new SqlParameter("@ParentContactNo", model.ParentContactNo ?? (object)DBNull.Value),
                    new SqlParameter("@ParentRelationship", model.ParentRelationship ?? (object)DBNull.Value),
                    new SqlParameter("@GuardianFirstName", model.GuardianFirstName ?? (object)DBNull.Value),
                    new SqlParameter("@GuardianMiddleName", model.GuardianMiddleName ?? (object)DBNull.Value),
                    new SqlParameter("@GuardianLastName", model.GuardianLastName ?? (object)DBNull.Value),
                    new SqlParameter("@GuardianContactNo", model.GuardianContactNo ?? (object)DBNull.Value),
                    new SqlParameter("@GuardianRelationship", model.GuardianRelationship ?? (object)DBNull.Value)
                };

                // Execute stored procedure
                await _db.Database.ExecuteSqlRawAsync(
                    "EXEC UpdateStudentFamily @Id, @ParentFirstName, @ParentMiddleName, @ParentLastName, @ParentContactNo, @ParentRelationship, @GuardianFirstName, @GuardianMiddleName, @GuardianLastName, @GuardianContactNo, @GuardianRelationship",
                    parameters);

                return Json(new { success = true, message = "Family information updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating family information for StudentId: {StudentId}. Exception: {Exception}", model.Id, ex);
                return Json(new { success = false, message = "Error updating family information: " + ex.Message });
            }
        }



        // POST: Update School Information
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Faculty")]
        public async Task<IActionResult> UpdateSchool(StudentEntity model)
        {
            try
            {
                // Create parameters for stored procedure
                var parameters = new[]
                {
                    new SqlParameter("@Id", model.Id),
                    new SqlParameter("@SchoolName", model.SchoolName ?? (object)DBNull.Value),
                    new SqlParameter("@SchoolAddress", model.SchoolAddress ?? (object)DBNull.Value),
                    new SqlParameter("@SchoolContact", model.SchoolContact ?? (object)DBNull.Value),
                    new SqlParameter("@SchoolType", model.SchoolType ?? (object)DBNull.Value),
                    new SqlParameter("@YearOfGraduation", model.YearOfGraduation),
                    new SqlParameter("@LRN", model.LRN ?? (object)DBNull.Value),
                    new SqlParameter("@GWA", model.GWA)
                };

                // Execute stored procedure
                await _db.Database.ExecuteSqlRawAsync(
                    "EXEC UpdateStudentSchool @Id, @SchoolName, @SchoolAddress, @SchoolContact, @SchoolType, @YearOfGraduation, @LRN, @GWA",
                    parameters);

                return Json(new { success = true, message = "School information updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError("Error updating school information for StudentId: {StudentId}. Exception: {Exception}", model.Id, ex);
                return Json(new { success = false, message = "Error updating school information: " + ex.Message });
            }
        }






        //unenroll student ----------------------------------------------------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Unenroll(int id, string ReasonDescription)
        {
            try
            {
                _logger.LogInformation("Unenrolling student ID: {Id} with reason: {Reason}", id, ReasonDescription);

                var connection = _db.Database.GetDbConnection();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "UnenrollStudent"; // name of your stored procedure
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    var paramId = command.CreateParameter();
                    paramId.ParameterName = "@StudentId";
                    paramId.Value = id;
                    command.Parameters.Add(paramId);

                    var paramReason = command.CreateParameter();
                    paramReason.ParameterName = "@ReasonDescription";
                    paramReason.Value = ReasonDescription;
                    command.Parameters.Add(paramReason);

                    if (connection.State != ConnectionState.Open)
                        await connection.OpenAsync();

                    await command.ExecuteNonQueryAsync();
                }

                TempData["SuccessMessage"] = "Student unenrolled successfully.";
                return RedirectToAction("Unenroll");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while unenrolling student with ID: {Id}", id);
                TempData["ErrorMessage"] = "An error occurred while unenrolling the student.";
                return RedirectToAction("StudentDetail", new { id });
            }
        }



        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Unenroll()
        {
            ViewData["ActiveMenu"] = "Unenroll";

            var unenrolledstudents = await _db.UnenrolledStudents
                .FromSqlRaw("EXEC GetUnenrolledStudents")
                .ToListAsync();

            return View("~/Views/AdminPortal/Unenroll.cshtml", unenrolledstudents);
        }



        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnenrollDetail(int Id)
        {
            ViewData["ActiveMenu"] = "UnenrollDetail";

            var unenrolledstudents = await _db.UnenrolledStudents
                .FromSqlRaw("EXEC GetByIdUnenrolledStudents @Id={0}", Id)
                .ToListAsync();

            return View("~/Views/AdminPortal/UnenrollDetail.cshtml", unenrolledstudents);
        }


        



        //Faculty-------------------------------------------------------------------------------------------------------------------------------
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Faculty()
        {
            ViewData["ActiveMenu"] = "Faculty";

            _logger.LogInformation("Faculty action called.");

            // Correctly set cache control headers to prevent browser caching
            Response.Headers.Append("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.Headers.Append("Pragma", "no-cache");
            Response.Headers.Append("Expires", "0");

            var faculty = await _userRepo.GetUsersByRole("Faculty");
            var gradingPeriods = await _db.GradingPeriod.ToListAsync(); // make sure to `await` if it's 

            var viewModel = new FacultyPageViewModel
            {
                Users = faculty,
                GradingPeriods = gradingPeriods,
            };

            return View(viewModel);
        }

        public IActionResult GradeStatusPartialView()
        {
            

            
            var gradingPeriod = _db.GradingPeriod.Where(g => g.PeriodStatus == "Closed").FirstOrDefault();

            return PartialView("GradeStatusPartialView", gradingPeriod);
        }


        [HttpPost]
        public IActionResult GradeStatusPartialView(int gradingPeriodID, string newStatus)
        {

            var gradingPeriod = _db.GradingPeriod.Find(gradingPeriodID);

            if (gradingPeriod == null)
            {
                _logger.LogWarning("GradingPeriod not found.");
                return NotFound();
            }

            gradingPeriod.PeriodStatus = newStatus; 

            _db.GradingPeriod.Update(gradingPeriod);
            _db.SaveChanges();

            

            return RedirectToAction("Faculty");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateFaculty(int id, string subject, string position)
        {
            try
            {
                var parameters = new[]
                {
            new SqlParameter("@Id", id),
            new SqlParameter("@Subject", subject ?? (object)DBNull.Value),
            new SqlParameter("@Position", position ?? (object)DBNull.Value)
        };

                await _db.Database.ExecuteSqlRawAsync(
                    "EXEC UpdateFaculty @Id, @Subject, @Position",
                    parameters);

                return Json(new { success = true, message = "Faculty details updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating faculty with ID {Id}", id);
                return Json(new { success = false, message = "An error occurred while updating faculty details." });
            }
        }


        [HttpGet]
        public IActionResult sp_SelectUserById(int id)
        {
            var faculty = _db.Users.FirstOrDefault(f => f.Id == id);
            if (faculty == null)
                return NotFound();

            return Json(new
            {
                id = faculty.Id,
                firstName = faculty.FirstName,
                lastName = faculty.LastName,
                email = faculty.Email,
                subject = faculty.Subject,
                position = faculty.Position
            });
        }






        //Accounts -----------------------------------------------------------------------------------------------------------------------------------

        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Accounts()
        {
            ViewData["ActiveMenu"] = "Accounts";

            // Correctly set cache control headers to prevent browser caching
            Response.Headers.Append("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.Headers.Append("Pragma", "no-cache");
            Response.Headers.Append("Expires", "0");

            var admins = await _userRepo.GetUsersByRole("Admin");
            return View(admins);
        }





        //Accounts & Faculty-------------------------------------------------------------------------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            try
            {
                // Option 1: Using stored procedure
                var result = await _db.Database.ExecuteSqlRawAsync("EXEC sp_SoftDeleteUser @Id = {0}", id);

                // Option 2: Using repository pattern (if you have a delete method in your repo)
                // var result = await _userRepo.SoftDeleteUser(id);

                if (result == 0)
                {
                    return Json(new { success = false, message = "User not found or already deleted." });
                }

                return Json(new { success = true, message = "User account deleted successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }












        //Profile-------------------------------------------------------------------------------------------------------------------------------


        [Authorize]
        public async Task<IActionResult> Profile()
        {
            ViewData["ActiveMenu"] = "Profile";

            var email = User.Identity?.Name;

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Account");
            }

            // ✅ Await the SQL call
            var users = await _db.Users
                .FromSqlRaw("EXEC GetUserByEmail @p0", email)
                .AsNoTracking()
                .ToListAsync();

            var profile = users.FirstOrDefault();

            if (profile == null)
            {
                return NotFound("User not found.");
            }

            return View(new List<UserEntity> { profile }); // If you're using IEnumerable<UserEntity> in the view
        }












        public IActionResult Signin()
        {
            ViewData["ActiveMenu"] = "Signin";
            return View();
        }





        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult Classes()
        {
            ViewData["ActiveMenu"] = "Classes";
            return View();
        }



        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult Schedule()
        {
            ViewData["ActiveMenu"] = "Schedule";
            return View();
        }



        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult Scheduleclass()
        {
            ViewData["ActiveMenu"] = "Scheduleclass";
            return View();
        }



        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult Subject()
        {
            ViewData["ActiveMenu"] = "Subject";
            return View();
        }



        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult EditInfo()
        {
            ViewData["ActiveMenu"] = "EditInfo";
            return View();
        }



        [Authorize(Roles = "Admin,Faculty")]
        public IActionResult SubjectLoads()
        {
            ViewData["ActiveMenu"] = "SubjectLoads";
            return View();
        }



        
    }
}
