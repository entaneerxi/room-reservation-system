using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomReservationSystem.Data;
using RoomReservationSystem.Models;

namespace RoomReservationSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RoomsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Rooms.OrderBy(r => r.Name).ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Room room, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    room.ImageUrl = await SaveImage(imageFile);
                }

                room.CreatedAt = DateTime.Now;
                room.UpdatedAt = DateTime.Now;
                _context.Add(room);
                await _context.SaveChangesAsync();
                TempData["Success"] = "เพิ่มห้องพักสำเร็จ";
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var room = await _context.Rooms.FindAsync(id);
            if (room == null) return NotFound();
            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Room room, IFormFile? imageFile)
        {
            if (id != room.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null)
                    {
                        room.ImageUrl = await SaveImage(imageFile);
                    }

                    room.UpdatedAt = DateTime.Now;
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "แก้ไขห้องพักสำเร็จ";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
                TempData["Success"] = "ลบห้องพักสำเร็จ";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool RoomExists(int id)
        {
            return _context.Rooms.Any(e => e.Id == id);
        }

        private async Task<string> SaveImage(IFormFile imageFile)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "rooms");
            Directory.CreateDirectory(uploadsFolder);
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return "/images/rooms/" + uniqueFileName;
        }
    }
}
