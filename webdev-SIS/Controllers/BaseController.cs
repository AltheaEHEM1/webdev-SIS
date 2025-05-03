using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace webdev_SIS.Controllers
{
    public class BaseController : Controller
    {
        protected string CurrentUserRole => HttpContext.Session.GetString("UserRole") ?? "";
        protected string CurrentUserName => HttpContext.Session.GetString("UserName") ?? "";
        protected int CurrentUserId => int.TryParse(HttpContext.Session.GetString("UserId"), out int id) ? id : 0;

        protected bool IsAdmin => CurrentUserRole == "Admin";
        protected bool IsFaculty => CurrentUserRole == "Faculty";
        protected bool IsAdmission => CurrentUserRole == "Admission";

        protected IActionResult RedirectToRoleHome()
        {
            return CurrentUserRole switch
            {
                "Admin" => RedirectToAction("Index", "AdminPortal"),
                "Faculty" => RedirectToAction("Index", "FacultyPortal"),
                "Admission" => RedirectToAction("Start", "AdmissionPortal"),
                _ => RedirectToAction("Login", "Account")
            };
        }

        // ✅ Add this method to disable browser caching of pages
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Disable browser caching
            Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            // Check if session is invalid (no user role or user is logged out)
            if (string.IsNullOrEmpty(CurrentUserRole) || CurrentUserId == 0)
            {
                // If no valid session, redirect to the login page
                context.Result = RedirectToAction("Login", "Account");
                return;
            }

            base.OnActionExecuting(context);
        }

    }
}
