using EduHome4.DAL;
using EduHome4.Models;
using EduHome4.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome4.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public HomeController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {

            HomeVM homeVM = new HomeVM
            {
                Sliders =await _db.Sliders.Where(x => !x.IsDeactive).ToListAsync(),
                About =await _db.Abouts.FirstAsync(),
                Services =await _db.Services.OrderByDescending(x=>x.Id).Where(x=> !x.IsDeactive).Take(3).ToListAsync(),
                Courses =await _db.Courses.Take(3).ToListAsync(),
                Testimonials = await _db.testimonials.ToListAsync(),              
                Blogs = await _db.Blogs.ToListAsync(),
                Events = await _db.Events.Take(4).ToListAsync(),
             
                

            };
            return View(homeVM);
        }

       
      
       
    }
}
