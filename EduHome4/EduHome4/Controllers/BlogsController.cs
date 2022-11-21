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
    public class BlogsController : Controller
    {
        private readonly AppDbContext _db;
        public BlogsController(AppDbContext db)
        {
            _db = db;
        }
        
        public async Task<IActionResult> IndexAsync()
        {

            List<Blog> blogs = await _db.Blogs.ToListAsync();
            return View(blogs);
        }
    }
}
