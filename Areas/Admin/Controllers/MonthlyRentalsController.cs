using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoomReservationSystem.Data;
using RoomReservationSystem.Models;

namespace RoomReservationSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class MonthlyRentalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MonthlyRentalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var rentals = await _context.MonthlyRentals
                .Include(m => m.Room)
                .Include(m => m.User)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
            return View(rentals);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var rental = await _context.MonthlyRentals
                .Include(m => m.Room)
                .Include(m => m.User)
                .Include(m => m.UtilityBills)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (rental == null) return NotFound();
            return View(rental);
        }

        public IActionResult Create()
        {
            ViewData["RoomId"] = new SelectList(_context.Rooms.Where(r => r.IsAvailable), "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MonthlyRental rental)
        {
            if (ModelState.IsValid)
            {
                rental.CreatedAt = DateTime.Now;
                rental.UpdatedAt = DateTime.Now;
                _context.Add(rental);
                await _context.SaveChangesAsync();
                TempData["Success"] = "บันทึกข้อมูลการเช่าสำเร็จ";
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", rental.RoomId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", rental.UserId);
            return View(rental);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var rental = await _context.MonthlyRentals.FindAsync(id);
            if (rental == null) return NotFound();
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", rental.RoomId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", rental.UserId);
            return View(rental);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MonthlyRental rental)
        {
            if (id != rental.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    rental.UpdatedAt = DateTime.Now;
                    _context.Update(rental);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "แก้ไขข้อมูลการเช่าสำเร็จ";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalExists(rental.Id))
                        return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", rental.RoomId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", rental.UserId);
            return View(rental);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EndRental(int id)
        {
            var rental = await _context.MonthlyRentals.FindAsync(id);
            if (rental != null)
            {
                rental.Status = "Ended";
                rental.EndDate = DateTime.Now;
                rental.UpdatedAt = DateTime.Now;
                _context.Update(rental);
                await _context.SaveChangesAsync();
                TempData["Success"] = "สิ้นสุดสัญญาเช่าสำเร็จ";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool RentalExists(int id)
        {
            return _context.MonthlyRentals.Any(e => e.Id == id);
        }
    }
}
