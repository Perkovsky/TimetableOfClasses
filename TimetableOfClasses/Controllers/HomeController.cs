using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimetableOfClasses.Models;
using TimetableOfClasses.Models.ViewModels;

namespace TimetableOfClasses.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ITimetableRepository repository;

        public HomeController(ITimetableRepository repository) => this.repository = repository;

        public IActionResult Index()
        {
            var timetables = repository.Timetables.GroupBy(g => new { g.Date, g.Group })
                .Select(g => new TimetableOfClassesViewModel
                {
                    Group = g.Key.Group,
                    Date = g.Key.Date,
                    Timetables = g.Select(t => new Timetable
                    {
                        Discipline = t.Discipline,
                        TimeBegin = t.TimeBegin,
                        TimeEnd = t.TimeEnd,
                        Teacher = t.Teacher
                    })
                    .OrderBy(t => t.TimeBegin)
                    .ToList()
                });

            return View(timetables);
        }
    }
}
