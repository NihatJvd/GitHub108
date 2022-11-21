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
    public class CoursesController : Controller
    {
        private readonly AppDbContext _db;      
        public CoursesController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.CourseCount = _db.Courses.Count();
            List<Course> courses = await _db.Courses.OrderByDescending(x=>x.Id).Take(6).ToListAsync();
            return View(courses);
        }
        public async Task<IActionResult> LoadMore(int skip)
        {
            List<Course> courses = await _db.Courses.OrderByDescending(x => x.Id).Skip(skip).Take(6).ToListAsync();

            return PartialView("_LoadMoreCoursesPartial", courses);
        }
    }
}
