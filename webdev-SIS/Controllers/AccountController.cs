// Update AccountController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using webdev_SIS.Services;
using webdev_SIS.DataLayer;
using System.Threading.Tasks;
using webdev_SIS.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Logging;

namespace webdev_SIS.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserRepository _userRepo;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ApplicationDbContext db, ILogger<AccountController> logger)
        {
            _userRepo = new UserRepository(db);
            _logger = logger;
        }

        // GET: Display signup form for Admission (default)
        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }

        // POST: Process Admission signup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(UserEntity user)
        {
            try
            {
                if (!IsPasswordValid(user.PasswordHash))
                {
                    ModelState.AddModelError("PasswordHash", "Password must be at least 12 characters and include uppercase, lowercase, number, and special character.");
                }

                if (ModelState.IsValid)
                {
                    user.Role = "Admission";
                    user.Status = "Active";

                    await _userRepo.AddUser(user);
                    TempData["SuccessMessage"] = "Account created successfully. Please log in.";
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during signup: {ex.Message}");
                ModelState.AddModelError("", "An error occurred during signup. Please try again.");
            }

            return View(user);
        }



        // POST: Process Faculty signup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Faculty(UserEntity user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    user.Role = "Faculty"; // Ensure role is set
                    user.Status = "Active";

                    await _userRepo.AddUser(user);
                    _logger.LogInformation($"New faculty user created: {user.Email}");

                    TempData["SuccessMessage"] = "Faculty account created successfully.";
                    return RedirectToAction("Faculty", "AdminPortal");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during faculty signup: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred during signup. Please try again.";
            }

            return RedirectToAction("Faculty", "AdminPortal");
        }




        // POST: Process Admin signup
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Admin(UserEntity user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    user.Role = "Admin"; // Ensure role is set
                    user.Status = "Active";

                    await _userRepo.AddUser(user);
                    _logger.LogInformation($"New admin user created: {user.Email}");

                    TempData["SuccessMessage"] = "Admin account created successfully.";
                    return RedirectToAction("Accounts", "AdminPortal");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during admin signup: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred during signup. Please try again.";
            }

            return RedirectToAction("Accounts", "AdminPortal");
        }



        // GET: Display login form
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }



        // POST: Process login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserEntity model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.PasswordHash))
                {
                    ModelState.AddModelError("", "Email and password are required.");
                    return View(model);
                }

                var userInDb = await _userRepo.GetUserByEmail(model.Email);

                // ✅ This checks raw password input against the stored hash
                if (userInDb != null && UserRepository.VerifyPassword(model.PasswordHash, userInDb.PasswordHash!))
                {
                    // Store session & login
                    HttpContext.Session.SetString("UserId", userInDb.Id.ToString());
                    HttpContext.Session.SetString("UserRole", userInDb.Role ?? "");
                    HttpContext.Session.SetString("UserName", userInDb.FirstName ?? "");

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userInDb.Email ?? ""),
                        new Claim(ClaimTypes.NameIdentifier, userInDb.Id.ToString()),
                        new Claim(ClaimTypes.Role, userInDb.Role ?? "")
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddHours(3)
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    _logger.LogInformation($"User logged in: {userInDb.Email}");

                    return userInDb.Role switch
                    {
                        "Admin" => RedirectToAction("Index", "AdminPortal"),
                        "Faculty" => RedirectToAction("Index", "FacultyPortal"),
                        "Admission" => RedirectToAction("Start", "AdmissionPortal"),
                        _ => RedirectToAction("Login")
                    };
                }

                _logger.LogWarning($"Failed login attempt for email: {model.Email}");
                ModelState.AddModelError("", "Invalid login credentials.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during login: {ex.Message}");
                ModelState.AddModelError("", "An error occurred during login. Please try again.");
            }

            return View(model);
        }









        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordEntity model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (string.IsNullOrWhiteSpace(model.NewPassword) || string.IsNullOrWhiteSpace(model.ConfirmPassword))
            {
                ModelState.AddModelError("", "Password fields cannot be empty.");
                return View(model);
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match.");
                return View(model);
            }

            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                ModelState.AddModelError("", "User session not found. Please log in again.");
                return RedirectToAction("Login");
            }

            var changeSuccess = await _userRepo.ChangePassword(userId, model.NewPassword!); // Use ! to tell compiler it's safe

            if (changeSuccess)
            {
                TempData["SuccessMessage"] = "Password changed successfully.";
                return RedirectToAction("Login");
            }

            ModelState.AddModelError("", "Error changing password.");
            return View(model);
        }





        private bool IsPasswordValid(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 12)
                return false;

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSymbol = password.Any(ch => !char.IsLetterOrDigit(ch));

            return hasUpper && hasLower && hasDigit && hasSymbol;
        }
























        public async Task<IActionResult> Logout()
        {
            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();

            // Clear any session data if necessary
            HttpContext.Session.Clear();

            // Redirect to the Login view
            return RedirectToAction("Login");
        }


    }
}