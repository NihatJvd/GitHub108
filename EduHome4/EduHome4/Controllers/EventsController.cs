using EduHome4.DAL;
using EduHome4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome4.Controllers
{
    public class EventsController : Controller
    {
        private readonly AppDbContext _db;
        public EventsController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.EventCount = _db.Events.Count();
            List<Event> events = await _db.Events.OrderByDescending(x=> x .Id).Take(6).ToListAsync();
            return View(events);
        }
        public async Task<IActionResult> LoadMoreEvent(int skip)
        {
            List<Event> events = await _db.Events.OrderByDescending(x => x.Id).Skip(skip).Take(6).ToListAsync();
            return PartialView("_LoadMoreEventPartial",events);
        }
    }
}
