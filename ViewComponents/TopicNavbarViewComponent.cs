using Microsoft.AspNetCore.Mvc;
using ParwatPiyushNewsPortal.Data; // adjust namespace
using System.Linq;
using System.Threading.Tasks;

namespace ParwatPiyushNewsPortal.ViewComponents
{
    public class TopicNavbarViewComponent: ViewComponent
    {
        private readonly ParwatPiyushDB _context;

        public TopicNavbarViewComponent(ParwatPiyushDB context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var topics = _context.Topics.OrderBy(t => t.Name).ToList();
            return View(topics);
        }
    }
}
