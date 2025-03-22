using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ParwatPiyushNewsPortal.Models;
using ParwatPiyushNewsPortal.Data;
using Microsoft.EntityFrameworkCore;

namespace ParwatPiyushNewsPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ParwatPiyushDB _context;
        public HomeController(ParwatPiyushDB context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var newsList = _context.News
                                   .Include(n => n.Topic)
                                   .Include(n => n.Author)
                                   .OrderByDescending(n => n.PublishedDate)
                                   .ToList();

            return View(newsList);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
