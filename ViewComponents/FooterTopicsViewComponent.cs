using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParwatPiyushNewsPortal.Data;
using ParwatPiyushNewsPortal.Models;
namespace ParwatPiyushNewsPortal.ViewComponents
{
    public class FooterTopicsViewComponent : ViewComponent
    {
        private readonly ParwatPiyushDB _context;

        public FooterTopicsViewComponent(ParwatPiyushDB context)
        {
            _context = context;
        }

        //    public async Task<IViewComponentResult> InvokeAsync()
        //    {
        //        var topics = await _context.Topics
        //                        .OrderBy(t => t.Name)
        //                        .Take(8)
        //                        .ToListAsync();

        //        var topicGroups = new List<List<Topics>>
        //{
        //    topics.Take(4).ToList(),
        //    topics.Skip(4).Take(4).ToList()
        //};

        //        return View(topicGroups);
        //    }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var topics = await _context.Topics
                .OrderBy(t => t.Name)
                .ToListAsync();

            return View(topics);
        }


    }
}
