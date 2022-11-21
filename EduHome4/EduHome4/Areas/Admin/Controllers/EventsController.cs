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
    public class EventsController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public EventsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Event> events = await _db.Events.ToListAsync();
            ; return View(events);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event even)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (even.Photo == null)
            {
                ModelState.AddModelError("Photo", "Xaiş Olunur Şəkil Fayıl-ı Seçin");
                return View();
            }
            if (!even.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Seçdiyiniz Fayıl,Düzgün deyildir Xaiş olunur Şəkil Fayıl-ı Seçin");
                return View();
            }
            if (even.Photo.IsOlderMaxMB())
            {
                ModelState.AddModelError("Photo", "Seçdiyiniz Şəkil 2 mb-dan Artıqdır Xaiş Olunur 2 mb Aşağı Şəkil Fayıl-ını Seçin");
                return View();
            }
            string path = Path.Combine(_env.WebRootPath, "img", "event");
            even.Image = await even.Photo.SaveFileAsync(path);
            await _db.Events.AddAsync(even);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Event even = await _db.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (even == null)
            {
                return BadRequest();
            }
            return View(even);
        }


        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Event even= await _db.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (even == null)
            {
                return BadRequest();
            }
            if (even.IsDeactive)
            {
                even.IsDeactive = false;
            }
            else
            {
                even.IsDeactive = true;
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
            Event even = await _db.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (even == null)
            {
                return BadRequest();
            }
            return View(even);
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
            Event even = await _db.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (even == null)
            {
                return BadRequest();
            }
            even.IsDeactive = true;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> UpdateAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Event dbEven = await _db.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (dbEven == null)
                return BadRequest();
            return View(dbEven);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Event even)
        {

            if (id == null)
            {
                return NotFound();
            }
            Event dbEven = await _db.Events.FirstOrDefaultAsync(x => x.Id == id);
            if (dbEven == null)
                return BadRequest();
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (even.Photo != null)
            {
                if (!even.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "Seçdiyiniz Fayıl,Düzgün deyildir Xaiş olunur Şəkil Fayıl-ı Seçin");
                    return View(dbEven);
                }
                if (even.Photo.IsOlderMaxMB())
                {
                    ModelState.AddModelError("Photo", "Seçdiyiniz Şəkil 2 mb-dan Artıqdır Xaiş Olunur 2 mb Aşağı Şəkil Fayıl-ını Seçin");
                    return View(dbEven);
                }
                string path = Path.Combine(_env.WebRootPath, "img", "event");
                dbEven.Image = await even.Photo.SaveFileAsync(path);
            }

            dbEven.Title = even.Title;
            dbEven.StartTime = even.StartTime;
            dbEven.EndTime = even.EndTime;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}