using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomReservationSystem.Data;
using RoomReservationSystem.Models;

namespace RoomReservationSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class FacilitiesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FacilitiesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Facilities.OrderBy(f => f.DisplayOrder).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Facility facility, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    facility.ImageUrl = await SaveImage(imageFile);
                }

                facility.CreatedAt = DateTime.Now;
                _context.Add(facility);
                await _context.SaveChangesAsync();
                TempData["Success"] = "เพิ่มสิ่งอำนวยความสะดวกสำเร็จ";
                return RedirectToAction(nameof(Index));
            }
            return View(facility);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var facility = await _context.Facilities.FindAsync(id);
            if (facility == null) return NotFound();
            return View(facility);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Facility facility, IFormFile? imageFile)
        {
            if (id != facility.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null)
                    {
                        facility.ImageUrl = await SaveImage(imageFile);
                    }

                    _context.Update(facility);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "แก้ไขสิ่งอำนวยความสะดวกสำเร็จ";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacilityExists(facility.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(facility);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var facility = await _context.Facilities.FindAsync(id);
            if (facility != null)
            {
                _context.Facilities.Remove(facility);
                await _context.SaveChangesAsync();
                TempData["Success"] = "ลบสิ่งอำนวยความสะดวกสำเร็จ";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool FacilityExists(int id)
        {
            return _context.Facilities.Any(e => e.Id == id);
        }

        private async Task<string> SaveImage(IFormFile imageFile)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "facilities");
            Directory.CreateDirectory(uploadsFolder);
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return "/images/facilities/" + uniqueFileName;
        }
    }
}
