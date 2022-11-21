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
    public class AboutController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;


        public AboutController(AppDbContext db ,IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            List<About> abouts = await _db.Abouts.ToListAsync();
            return View(abouts);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(About about)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool isExist = await _db.Abouts.AnyAsync(x => x.Title == about.Title);
            if (isExist)
            {
                ModelState.AddModelError("Title", "About is available in this Name");

                return View();
            }
            await _db.Abouts.AddAsync(about);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            About about = await _db.Abouts.FirstOrDefaultAsync(x => x.Id == id);
            if (about == null)
            {
                return BadRequest();
            }
            return View(about);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            About about = await _db.Abouts.FirstOrDefaultAsync(x => x.Id == id);
            if (about == null)
            {
                return BadRequest();
            }
            return View(about);
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
            About about = await _db.Abouts.FirstOrDefaultAsync(x => x.Id == id);
            if (about == null)
            {
                return BadRequest();
            }
            about.IsDeactive = true;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            About about = await _db.Abouts.FirstOrDefaultAsync(x => x.Id == id);
            if (about == null)
            {
                return BadRequest();
            }
            if (about.IsDeactive)
            {
                about.IsDeactive = false;
            }
            else
            {
                about.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> UpdateAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            About dbAbout = await _db.Abouts.FirstOrDefaultAsync(x => x.Id == id);
            if (dbAbout == null)
                return BadRequest();
            return View(dbAbout);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, About about)
        {

            if (id == null)
            {
                return NotFound();
            }
            About dbAbout = await _db.Abouts.FirstOrDefaultAsync(x => x.Id == id);
            if (dbAbout == null)
                return BadRequest();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (about.Photo != null)
            {
                if (!about.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Seçdiyiniz Fayıl,Düzgün deyildir Xaiş olunur Şəkil Fayıl-ı Seçin");
                    return View(dbAbout);
                }
                if (about.Photo.IsOlderMaxMB())
                {
                    ModelState.AddModelError("Photo", "Seçdiyiniz Şəkil 2 mb-dan Artıqdır Xaiş Olunur 2 mb Aşağı Şəkil Fayıl-ını Seçin");
                    return View(dbAbout);
                }
                string path = Path.Combine(_env.WebRootPath, "img", "about");
                dbAbout.Image = await about.Photo.SaveFileAsync(path);
            }

            dbAbout.Title = about.Title;
            dbAbout.Description =   about.Description;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
