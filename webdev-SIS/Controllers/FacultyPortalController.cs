using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webdev_SIS.Controllers;
using webdev_SIS.DataLayer;
using webdev_SIS.Models;
namespace system_SIS.Controllers
{
    public class FacultyPortalController : BaseController
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<FacultyPortalController> _logger;
        public FacultyPortalController(ApplicationDbContext db, ILogger<FacultyPortalController> logger)
        {
            _db = db;
            _logger = logger;
        }



        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "Home";
            var announcements = await _db.Announcements
               .FromSqlRaw("EXEC GetAllAnnouncements")
               .ToListAsync();

            var sortedAnnouncements = announcements.OrderByDescending(a => a.DatePosted).ToList();

            return View("~/Views/FacultyPortal/Index.cshtml", sortedAnnouncements);
        }

        public IActionResult Subject()
        {
            ViewData["ActiveMenu"] = "Subject";
            return View();
        }

        public IActionResult MasterList()
        {
            ViewData["ActiveMenu"] = "MasterList";
            return View();
        }

        public IActionResult Grades()
        {
            ViewData["ActiveMenu"] = "Grades";
            return View();
        }

        public IActionResult Forms()
        {
            ViewData["ActiveMenu"] = "Forms";
            return View();
        }

        public IActionResult ViewClassAd()
        {
            ViewData["ActiveMenu"] = "Home";
            return View();
        }
        public IActionResult ListReportCard()
        {
            ViewData["ActiveMenu"] = "Home";
            return View();
        }
        public IActionResult CardGradeReport()
        {
            ViewData["ActiveMenu"] = "Grades";
            return View();
        }

        public IActionResult EncodeGrades()
        {
            
            _logger.LogInformation("EncodeGrades action method called.");






            ViewData["ActiveMenu"] = "Grades";
            return View();
        }

        public IActionResult ChangeRG()
        {
            ViewData["ActiveMenu"] = "Grades";
            return View();
        }

        public IActionResult RCgrades()
        {
            ViewData["ActiveMenu"] = "Grades";
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