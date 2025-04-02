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
        //public IActionResult Index()
        //{
        //    var newsList = _context.News
        //                           .Include(n => n.Topic)
        //                           .Include(n => n.Author)
        //                           .OrderByDescending(n => n.PublishedDate)
        //                           .ToList();

        //    return View(newsList);
        //}
        public async Task<IActionResult> Index()
        {
            var latestNews = await _context.News.OrderByDescending(n => n.PublishedDate).Take(5).ToListAsync();
            var topics = await _context.Topics.Include(t => t.News).ToListAsync();

            //var newsByTopic = topics.ToDictionary(topic => topic.Name, topic => topic.News.OrderByDescending(n => n.PublishedDate).Take(4).ToList());
            var newsByTopic = topics
                    .GroupBy(topic => topic.Name) // Group by topic name to prevent duplicates
                    .ToDictionary(
                        group => group.Key, // Unique topic name as key
                        group => group.First().News.OrderByDescending(n => n.PublishedDate).Take(4).ToList() // Take news from the first occurrence
                    );

            ViewBag.LatestNews = latestNews;
            ViewBag.NewsByTopic = newsByTopic;

            return View();
        }

        public static string TruncateContent(string content, int wordLimit)
        {
            if (string.IsNullOrWhiteSpace(content)) return "";

            var words = content.Split(' ').Take(wordLimit);
            return string.Join(" ", words) + "...";
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
        public IActionResult Details(int id)
        //public async Task<IActionResult> Details(int id)
        {
            //var news = await _context.News.FindAsync(id);
            var news = _context.News.Include(n => n.Topic).FirstOrDefault(n => n.Id == id);
            if (news == null) return NotFound();
            return View(news);
        }
        public IActionResult Topic(string name)
        {
            var newsList = _context.News
                .Include(n => n.Topic)
                .Where(n => n.Topic.Name == name)
                .OrderByDescending(n => n.PublishedDate)
                .ToList(); 

            return View(newsList);
        }


    }
}
