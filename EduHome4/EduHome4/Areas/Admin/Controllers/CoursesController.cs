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
    public class CoursesController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public CoursesController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Course> courses = await _db.Courses.ToListAsync();
            ; return View(courses);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Course course)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (course.Photo == null)
            {
                ModelState.AddModelError("Photo", "Xaiş Olunur Şəkil Fayıl-ı Seçin");
                return View();
            }
            if (!course.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Seçdiyiniz Fayıl,Düzgün deyildir Xaiş olunur Şəkil Fayıl-ı Seçin");
                return View();
            }
            if (course.Photo.IsOlderMaxMB())
            {
                ModelState.AddModelError("Photo", "Seçdiyiniz Şəkil 2 mb-dan Artıqdır Xaiş Olunur 2 mb Aşağı Şəkil Fayıl-ını Seçin");
                return View();
            }
            string path = Path.Combine(_env.WebRootPath, "img", "course");
            course.Image = await course.Photo.SaveFileAsync(path);
            await _db.Courses.AddAsync(course);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Course course = await _db.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (course == null)
            {
                return BadRequest();
            }
            return View(course);
        }


        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Course course = await _db.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (course == null)
            {
                return BadRequest();
            }
            if (course.IsDeactive)
            {
                course.IsDeactive = false;
            }
            else
            {
                course.IsDeactive = true;
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
            Course course = await _db.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (course == null)
            {
                return BadRequest();
            }
            return View(course);
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
            Course course = await _db.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (course   == null)
            {
                return BadRequest();
            }
            course.IsDeactive = true;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> UpdateAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Course dbCourse = await _db.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCourse == null)
                return BadRequest();
            return View(dbCourse);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Course course)
        {

            if (id == null)
            {
                return NotFound();
            }
            Course dbCourse = await _db.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (dbCourse == null)
                return BadRequest();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (course.Photo != null)
            {
                if (!course.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Seçdiyiniz Fayıl,Düzgün deyildir Xaiş olunur Şəkil Fayıl-ı Seçin");
                    return View(dbCourse);
                }
                if (course.Photo.IsOlderMaxMB())
                {
                    ModelState.AddModelError("Photo", "Seçdiyiniz Şəkil 2 mb-dan Artıqdır Xaiş Olunur 2 mb Aşağı Şəkil Fayıl-ını Seçin");
                    return View(dbCourse);
                }
                string path = Path.Combine(_env.WebRootPath, "img", "course");
                dbCourse.Image = await course.Photo.SaveFileAsync(path);
            }

            dbCourse.Title = course.Title;
            dbCourse.Description = course.Description;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
