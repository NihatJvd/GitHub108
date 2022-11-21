using EduHome4.DAL;
using EduHome4.Helpers;
using EduHome4.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome4.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public BlogsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Blog> blogs = await _db.Blogs.ToListAsync();
            ; return View(blogs);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (blog.Photo == null)
            {
                ModelState.AddModelError("Photo", "Xaiş Olunur Şəkil Fayıl-ı Seçin");
                return View();
            }
            if (!blog.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Seçdiyiniz Fayıl,Düzgün deyildir Xaiş olunur Şəkil Fayıl-ı Seçin");
                return View();
            }
            if (blog.Photo.IsOlderMaxMB())
            {
                ModelState.AddModelError("Photo", "Seçdiyiniz Şəkil 2 mb-dan Artıqdır Xaiş Olunur 2 mb Aşağı Şəkil Fayıl-ını Seçin");
                return View();
            }
            string path = Path.Combine(_env.WebRootPath, "img", "blog");
            blog.Image = await blog.Photo.SaveFileAsync(path);
            await _db.Blogs.AddAsync(blog);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Blog blog = await _db.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (blog == null)
            {
                return BadRequest();
            }
            return View(blog);
        }


        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Blog blog = await _db.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (blog == null)
            {
                return BadRequest();
            }
            if (blog.IsDeactive)
            {
                blog.IsDeactive = false;
            }
            else
            {
                blog.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");


        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Blog blog = await _db.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (blog == null)
            {
                return BadRequest();
            }
            return View(blog);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]

        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Blog blog = await _db.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (blog == null)
            {
                return BadRequest();
            }
            blog.IsDeactive = true;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> UpdateAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Blog dbBlog = await _db.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (dbBlog == null)
                return BadRequest();
            return View(dbBlog);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Blog blog)
        {

            if (id == null)
            {
                return NotFound();
            }
            Blog dbBlog = await _db.Blogs.FirstOrDefaultAsync(x => x.Id == id);
            if (dbBlog == null)
                return BadRequest();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (blog.Photo != null)
            {
                if (!blog.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Seçdiyiniz Fayıl,Düzgün deyildir Xaiş olunur Şəkil Fayıl-ı Seçin");
                    return View(dbBlog);
                }
                if (blog.Photo.IsOlderMaxMB())
                {
                    ModelState.AddModelError("Photo", "Seçdiyiniz Şəkil 2 mb-dan Artıqdır Xaiş Olunur 2 mb Aşağı Şəkil Fayıl-ını Seçin");
                    return View(dbBlog);
                }
                string path = Path.Combine(_env.WebRootPath, "img", "blog");
                dbBlog.Image = await blog.Photo.SaveFileAsync(path);
            }

            dbBlog.Tittle = blog.Tittle;
            dbBlog.CreateTime = blog.CreateTime;
            dbBlog.By = dbBlog.By;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
