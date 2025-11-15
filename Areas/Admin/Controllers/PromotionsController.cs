using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomReservationSystem.Data;
using RoomReservationSystem.Models;

namespace RoomReservationSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class PromotionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PromotionsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Promotions.OrderByDescending(p => p.CreatedAt).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Promotion promotion, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    promotion.ImageUrl = await SaveImage(imageFile);
                }

                promotion.CreatedAt = DateTime.Now;
                promotion.UpdatedAt = DateTime.Now;
                _context.Add(promotion);
                await _context.SaveChangesAsync();
                TempData["Success"] = "เพิ่มโปรโมชั่นสำเร็จ";
                return RedirectToAction(nameof(Index));
            }
            return View(promotion);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var promotion = await _context.Promotions.FindAsync(id);
            if (promotion == null) return NotFound();
            return View(promotion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Promotion promotion, IFormFile? imageFile)
        {
            if (id != promotion.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null)
                    {
                        promotion.ImageUrl = await SaveImage(imageFile);
                    }

                    promotion.UpdatedAt = DateTime.Now;
                    _context.Update(promotion);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "แก้ไขโปรโมชั่นสำเร็จ";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PromotionExists(promotion.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(promotion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var promotion = await _context.Promotions.FindAsync(id);
            if (promotion != null)
            {
                _context.Promotions.Remove(promotion);
                await _context.SaveChangesAsync();
                TempData["Success"] = "ลบโปรโมชั่นสำเร็จ";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool PromotionExists(int id)
        {
            return _context.Promotions.Any(e => e.Id == id);
        }

        private async Task<string> SaveImage(IFormFile imageFile)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "promotions");
            Directory.CreateDirectory(uploadsFolder);
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return "/images/promotions/" + uniqueFileName;
        }
    }
}
