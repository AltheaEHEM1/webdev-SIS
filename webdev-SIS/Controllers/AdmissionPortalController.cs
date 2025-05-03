using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Diagnostics;
using webdev_SIS.DataLayer;
using webdev_SIS.Models;


namespace webdev_SIS.Controllers
{

    public class AdmissionPortalController(ApplicationDbContext db, ILogger<AdmissionPortalController> logger) : BaseController
    {
        private readonly ApplicationDbContext _db = db;
        private readonly ILogger<AdmissionPortalController> _logger = logger;

        // START PAGE
        public IActionResult Start()
        {
            ViewData["ActiveMenu"] = "Start";
            var model = HttpContext.Session.GetObject<AdmissionEntity>("AdmissionData") ?? new AdmissionEntity();
            return View(model);
        }
        [HttpPost]
        public IActionResult Start(AdmissionEntity model)
        {
            if (ModelState.IsValid)
            {
                var existingData = HttpContext.Session.GetObject<AdmissionEntity>("AdmissionData") ?? new AdmissionEntity();

                existingData.GradeLevel = model.GradeLevel;


                HttpContext.Session.SetObject("AdmissionData", existingData);

                return RedirectToAction("Index");
            }

            return View(model);
        }


        public IActionResult Index()
        {
            ViewData["ActiveMenu"] = "Index";
            var model = HttpContext.Session.GetObject<AdmissionEntity>("AdmissionData") ?? new AdmissionEntity();
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(AdmissionEntity model)
        {
            if (ModelState.IsValid)
            {
                var existingData = HttpContext.Session.GetObject<AdmissionEntity>("AdmissionData") ?? new AdmissionEntity();

                // Update personal information properties
                existingData.FirstName = model.FirstName;
                existingData.MiddleName = model.MiddleName;
                existingData.LastName = model.LastName;
                existingData.Suffix = model.Suffix;
                existingData.Email = model.Email;
                existingData.DateOfBirth = model.DateOfBirth;
                existingData.CivilStatus = model.CivilStatus;
                existingData.Sex = model.Sex;
                existingData.Country = model.Country;
                existingData.Region = model.Region;
                existingData.City = model.City;
                existingData.Height = model.Height;
                existingData.Weight = model.Weight;
                existingData.Religion = model.Religion;
                existingData.Disability = model.Disability;

                HttpContext.Session.SetObject("AdmissionData", existingData);

                return RedirectToAction("Contact"); // ✅ Should go to Contact
            }

            return View(model);
        }



        public IActionResult Contact()
        {
            ViewData["ActiveMenu"] = "Contact";
            var model = HttpContext.Session.GetObject<AdmissionEntity>("AdmissionData") ?? new AdmissionEntity();
            return View(model);
        }

        [HttpPost]
        public IActionResult Contact(AdmissionEntity model)
        {
            if (ModelState.IsValid)
            {
                var existingData = HttpContext.Session.GetObject<AdmissionEntity>("AdmissionData") ?? new AdmissionEntity();

                // Update contact information
                existingData.Landline_number = model.Landline_number;
                existingData.Phone_number = model.Phone_number;
                existingData.Emergency_landline_number = model.Emergency_landline_number;
                existingData.ContactPerson = model.ContactPerson;
                existingData.ContactNumber = model.ContactNumber;
                existingData.Relationship = model.Relationship;
                existingData.HouseNo = model.HouseNo;
                existingData.Barangay = model.Barangay;
                existingData.Street = model.Street;
                existingData.Municipality = model.Municipality;
                existingData.Province = model.Province;
                existingData.ZipCode = model.ZipCode;
                existingData.PermanentHouseNo = model.PermanentHouseNo;
                existingData.PermanentBarangay = model.PermanentBarangay;
                existingData.PermanentStreet = model.PermanentStreet;
                existingData.PermanentMunicipality = model.PermanentMunicipality;
                existingData.PermanentProvince = model.PermanentProvince;
                existingData.PermanentZipCode = model.PermanentZipCode;

                HttpContext.Session.SetObject("AdmissionData", existingData);
                return RedirectToAction("Family");
            }
            return View(model);
        }


        public IActionResult Family()
        {
            ViewData["ActiveMenu"] = "Family";
            var model = HttpContext.Session.GetObject<AdmissionEntity>("AdmissionData") ?? new AdmissionEntity();
            return View(model);
        }

        [HttpPost]
        public IActionResult Family(AdmissionEntity model)
        {
            if (ModelState.IsValid)
            {
                var existingData = HttpContext.Session.GetObject<AdmissionEntity>("AdmissionData") ?? new AdmissionEntity();

                // Update family information
                existingData.ParentFirstName = model.ParentFirstName;
                existingData.ParentMiddleName = model.ParentMiddleName;
                existingData.ParentLastName = model.ParentLastName;
                existingData.ParentContactNo = model.ParentContactNo;
                existingData.ParentRelationship = model.ParentRelationship;
                existingData.GuardianFirstName = model.GuardianFirstName;
                existingData.GuardianMiddleName = model.GuardianMiddleName;
                existingData.GuardianLastName = model.GuardianLastName;
                existingData.GuardianContactNo = model.GuardianContactNo;
                existingData.GuardianRelationship = model.GuardianRelationship;

                HttpContext.Session.SetObject("AdmissionData", existingData);
                return RedirectToAction("School");
            }
            return View(model);
        }


        public IActionResult School()
        {
            ViewData["ActiveMenu"] = "School5";
            var model = HttpContext.Session.GetObject<AdmissionEntity>("AdmissionData") ?? new AdmissionEntity();
            return View(model);
        }

        [HttpPost]
        public IActionResult School(AdmissionEntity model)
        {
            if (ModelState.IsValid)
            {
                var existingData = HttpContext.Session.GetObject<AdmissionEntity>("AdmissionData") ?? new AdmissionEntity();

                // Update school information
                existingData.SchoolName = model.SchoolName;
                existingData.SchoolAddress = model.SchoolAddress;
                existingData.SchoolContact = model.SchoolContact;
                existingData.SchoolType = model.SchoolType;
                existingData.YearOfGraduation = model.YearOfGraduation;
                existingData.LRN = model.LRN;
                existingData.GWA = model.GWA;

                HttpContext.Session.SetObject("AdmissionData", existingData);
                return RedirectToAction("Finish");
            }
            return View(model);
        }



        public IActionResult Finish()
        {
            //ViewData["ActiveMenu"] = "Finish";
            var model = HttpContext.Session.GetObject<AdmissionEntity>("AdmissionData");

            //return Ok (model);

            if (model == null)
            {
                return RedirectToAction("Submit_application");
            }
            else
            {
                return View(model);
            }

            //return Ok(View(model));


        }


        [HttpPost]
        public async Task<IActionResult> Finish(AdmissionEntity model)
        {
            //return Ok("model");

            _logger.LogInformation("Submit_application method triggered.");

            var admissionData = HttpContext.Session.GetObject<AdmissionEntity>("AdmissionData");

            //return Ok(admissionData == null);

            if (admissionData == null)
            {
                _logger.LogDebug("Admission data is null when submitting the application.");
                return RedirectToAction("Finish");

            }

            admissionData.TermsAccepted = model.TermsAccepted;

            try
            {

                await _db.Database.ExecuteSqlInterpolatedAsync($@"
                    EXEC CreateAdmissions 
                    @GradeLevel={admissionData.GradeLevel}, 
                    @FirstName={admissionData.FirstName}, 
                    @MiddleName={admissionData.MiddleName}, 
                    @LastName={admissionData.LastName}, 
                    @Suffix={admissionData.Suffix}, 
                    @Email={admissionData.Email}, 
                    @DateOfBirth={admissionData.DateOfBirth}, 
                    @CivilStatus={admissionData.CivilStatus}, 
                    @Sex={admissionData.Sex}, 
                    @Country={admissionData.Country}, 
                    @Region={admissionData.Region}, 
                    @City={admissionData.City}, 
                    @Height={admissionData.Height}, 
                    @Weight={admissionData.Weight}, 
                    @Religion={admissionData.Region}, 
                    @Disability={admissionData.Disability}, 
                    @Phone_number={admissionData.Phone_number}, 
                    @Landline_number={admissionData.Landline_number}, 
                    @Emergency_landline_number={admissionData.Emergency_landline_number}, 
                    @ContactPerson={admissionData.ContactPerson}, 
                    @ContactNumber={admissionData.ContactNumber}, 
                    @Relationship={admissionData.Relationship}, 
                    @HouseNo={admissionData.HouseNo}, 
                    @Barangay={admissionData.Barangay}, 
                    @Street={admissionData.Street}, 
                    @Municipality={admissionData.Municipality}, 
                    @Province={admissionData.Province}, 
                    @ZipCode={admissionData.ZipCode}, 
                    @PermanentHouseNo={admissionData.PermanentHouseNo}, 
                    @PermanentBarangay={admissionData.PermanentBarangay}, 
                    @PermanentStreet={admissionData.PermanentStreet}, 
                    @PermanentMunicipality={admissionData.PermanentMunicipality}, 
                    @PermanentProvince={admissionData.PermanentProvince}, 
                    @PermanentZipCode={admissionData.PermanentZipCode}, 
                    @ParentFirstName={admissionData.ParentFirstName}, 
                    @ParentMiddleName={admissionData.ParentMiddleName}, 
                    @ParentLastName={admissionData.ParentLastName}, 
                    @ParentContactNo={admissionData.ParentContactNo}, 
                    @ParentRelationship={admissionData.ParentRelationship}, 
                    @GuardianFirstName={admissionData.GuardianFirstName}, 
                    @GuardianMiddleName={admissionData.GuardianMiddleName}, 
                    @GuardianLastName={admissionData.GuardianLastName}, 
                    @GuardianContactNo={admissionData.GuardianContactNo}, 
                    @GuardianRelationship={admissionData.GuardianRelationship}, 
                    @SchoolName={admissionData.SchoolName}, 
                    @SchoolAddress={admissionData.SchoolAddress}, 
                    @SchoolContact={admissionData.SchoolContact}, 
                    @SchoolType={admissionData.SchoolType}, 
                    @YearOfGraduation={admissionData.YearOfGraduation}, 
                    @LRN={admissionData.LRN}, 
                    @GWA={admissionData.GWA},
                    @TermsAccepted={admissionData.TermsAccepted}
                   
                ");

                _logger.LogDebug("Admission data saved successfully.");
                HttpContext.Session.Remove("AdmissionData");

                TempData["SuccessMessage"] = "Your application has been submitted successfully!";
                return RedirectToAction("SubmissionSuccess");
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "Database operation failed."); // Move this before return

                return Ok(new { ex.Message });
            }

        }

        public IActionResult SubmissionSuccess()
        {
            return View();
        }







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

    }
}

