using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParwatPiyushNewsPortal.Data;
using ParwatPiyushNewsPortal.Models;
using System.Diagnostics;

namespace ParwatPiyushNewsPortal.Controllers
{
    [Authorize]
    public class AuthorController : Controller
    {
        private readonly ParwatPiyushDB _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AuthorController(ParwatPiyushDB context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
       
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateNews()
        {
            Debug.WriteLine("User accessing CreateNews: " + User.Identity.Name);
            Debug.WriteLine("User Authenticated: " + User.Identity.IsAuthenticated);
            Debug.WriteLine("User Role: " + User.FindFirst(ClaimTypes.Role)?.Value);
            Debug.WriteLine("Successfull login to createnews");
            if (!User.Identity.IsAuthenticated || User.FindFirst(ClaimTypes.Role)?.Value != "Admin")
            {
                Debug.WriteLine("❌ Unauthorized access attempt!");
                return RedirectToAction("Login", "Account");
            }

            Debug.WriteLine("✅ Access granted to CreateNews.");
            var topics = _context.Topics.ToList();
            ViewBag.Topics = topics;
            
            return View();
        }

       
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateNews(News news, IFormFile ImageFile)
        {
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("ModelState is invalid!");
                foreach (var error in ModelState)
                {
                    Debug.WriteLine($"Key: {error.Key}, Error: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
                ViewBag.Topics = _context.Topics.ToList();
                return View(news); // Return the form with validation errors
            }

            Debug.WriteLine("Attempting to save news...");

            // Ensure AuthorId is set from the logged-in user
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                Debug.WriteLine("User is not authenticated!");
                ModelState.AddModelError("", "User must be logged in.");
                return View(news);
            }

            news.AuthorId = int.Parse(userIdClaim.Value); // Assign the AuthorId

            // Handle image upload
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder); // Ensure directory exists

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                news.ImagePath = "/uploads/" + uniqueFileName; // Store relative path
            }
            else
            {
                Debug.WriteLine("No image uploaded, setting default image.");
               // news.ImagePath = "/uploads/default.jpg"; // Assign a default image if none uploaded
            }
            var username = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Username == username);

           
            if (user != null)
            {
                news.PublishedDate = DateTime.Now;
                //news.AuthorId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                news.AuthorId = user.Id;
                news.CreatedBy = user.Username; 
                _context.News.Add(news);
                _context.SaveChanges();
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "User not found. Please log in again.");
            }
        

              return View(news);

            //Debug.WriteLine("News saved successfully!");
            //return RedirectToAction("Index", "Home");
        }

    }
}
