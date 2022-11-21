using EduHome4.DAL;
using EduHome4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EduHome4.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ServicesController : Controller
    {
        private readonly AppDbContext _db;

        public ServicesController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            List<Service> services = await _db.Services.ToListAsync();
            return View(services);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service service)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool isExist = await _db.Services.AnyAsync(x => x.Title == service.Title);
            if (isExist)
            {
                ModelState.AddModelError("Title", "Bu Ad-da Servis Mövcuddur,Xaiş Olunur Başqa Ad Daxil Edəsiniz");

                return View();
            }
            await _db.Services.AddAsync(service);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Service service = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (service == null)
            {
                return BadRequest();
            }
            return View(service);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Service service = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (service == null)
            {
                return BadRequest();
            }
            return View(service);
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
            Service service = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (service == null)
            {
                return BadRequest();
            }
            service.IsDeactive = true;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Activity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Service service = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);
            if (service == null)
            {
                return BadRequest();
            }
            if (service.IsDeactive)
            {
                service.IsDeactive = false;
            }
            else
            {
                service.IsDeactive = true;
            }
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            Service service = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);
            if(service == null)
            {
                return BadRequest();
            }
            return View(service);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,Service service)
        {
            if(id == null)
            {
                return NotFound();
            }
            Service dbServices = await _db.Services.FirstOrDefaultAsync(x => x.Id == id);
            if(dbServices == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return View(dbServices);
            }
            bool isExist = await _db.Services.AnyAsync(x => x.Title == service.Title&&x.Id!=id);
            if (isExist)
            {
                ModelState.AddModelError("Title", "Bu Ad-da Servis Mövcuddur,Xaiş Olunur Başqa Ad Daxil Edəsiniz");

                return View(dbServices);
            }
            dbServices.Title = service.Title;
            dbServices.Description = service.Description;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");

        }
    }
}
