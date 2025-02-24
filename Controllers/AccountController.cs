using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Threading.Tasks;
using ParwatPiyushNewsPortal.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace ParwatPiyushNewsPortal.Controllers
{
    public class AccountController : Controller
    {
        private readonly ParwatPiyushDB _context;
        public AccountController(ParwatPiyushDB context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Username == username);
            //if (user == null || user.PasswordHash != password)
            //{
            //    ViewBag.Message = "Invalid login!";
            //    return View();
            //}
            Debug.WriteLine("User Input - Username: " + username);
            Debug.WriteLine("User Input - Password: " + password);
            if (user == null)
            {
                Debug.WriteLine("User Not Found!");
                ViewBag.Message = "User not found!";
                return View();
            }
            if (user.PasswordHash != password)
            {
                Debug.WriteLine("Incorrect Password!");
                ViewBag.Message = "Incorrect password!";
                return View();
            }
            Debug.WriteLine("User Found: " + user.Username);
            Debug.WriteLine("Stored Password: " + user.PasswordHash);
            Debug.WriteLine("Role: " + user.Role);

            string sessionId = Guid.NewGuid().ToString();
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("SessionId", sessionId)
        };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            Debug.WriteLine("User Role from Claims: " + User.FindFirst(ClaimTypes.Role)?.Value);
            Debug.WriteLine("User Claims: ");
            foreach (var claim in claims)
            {
                Debug.WriteLine($"{claim.Type}: {claim.Value}");
            }

            Debug.WriteLine("1Login Successful for " + user.Username);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
            {
                IsPersistent = true, // Keep session active
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
            });
            Debug.WriteLine("User successfully logged in!");
            HttpContext.Session.SetString("SessionId", sessionId);
            //return RedirectToAction("CreateNews", "Author");
            if (user.Role == "Admin")

            //if (user.Username == "admin")
            {
                Debug.WriteLine("Login Successful for " + user.Username);
                return RedirectToAction("CreateNews", "Author");
                //return Redirect("~/Author/CreateNews");

            }
            else
            {
                Debug.WriteLine("Login Successful failed ");
                //return RedirectToAction("Index", "Home");
                return RedirectToAction("CreateNews", "Author");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        [Authorize]
        public IActionResult CheckAuth()
        {
            Debug.WriteLine("User is authenticated!");
            Debug.WriteLine("User Name: " + User.Identity.Name);
            Debug.WriteLine("User Role: " + User.FindFirst(ClaimTypes.Role)?.Value);
            Debug.WriteLine("Authenticated: " + User.Identity.IsAuthenticated);
            foreach (var claim in User.Claims)
            {
                Debug.WriteLine($"Claim: {claim.Type} - {claim.Value}");
            }

            //return Content("Check console for claims...");
            return Content("Authenticated: " + User.Identity.IsAuthenticated +
                   " | Name: " + User.Identity.Name +
                   " | Role: " + User.FindFirst(ClaimTypes.Role)?.Value);

        }

    }
}