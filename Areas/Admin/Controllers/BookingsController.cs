using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomReservationSystem.Data;

namespace RoomReservationSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? status)
        {
            var bookings = _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .Include(b => b.Promotion)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                bookings = bookings.Where(b => b.Status == status);
            }

            ViewBag.SelectedStatus = status;
            return View(await bookings.OrderByDescending(b => b.CreatedAt).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .Include(b => b.Promotion)
                .Include(b => b.Payments)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (booking == null) return NotFound();
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPayment(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            booking.PaymentConfirmed = true;
            booking.PaymentDate = DateTime.Now;
            booking.UpdatedAt = DateTime.Now;

            _context.Update(booking);
            await _context.SaveChangesAsync();

            TempData["Success"] = "ยืนยันการชำระเงินสำเร็จ";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            if (!booking.PaymentConfirmed)
            {
                TempData["Error"] = "กรุณายืนยันการชำระเงินก่อน";
                return RedirectToAction(nameof(Details), new { id });
            }

            booking.Status = "Confirmed";
            booking.UpdatedAt = DateTime.Now;

            _context.Update(booking);
            await _context.SaveChangesAsync();

            TempData["Success"] = "ยืนยันการจองสำเร็จ";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckIn(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            if (booking.Status != "Confirmed")
            {
                TempData["Error"] = "การจองยังไม่ได้รับการยืนยัน";
                return RedirectToAction(nameof(Details), new { id });
            }

            booking.Status = "CheckedIn";
            booking.ActualCheckInDate = DateTime.Now;
            booking.UpdatedAt = DateTime.Now;

            _context.Update(booking);
            await _context.SaveChangesAsync();

            TempData["Success"] = "เช็คอินสำเร็จ";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOut(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            if (booking.Status != "CheckedIn")
            {
                TempData["Error"] = "ยังไม่ได้เช็คอิน";
                return RedirectToAction(nameof(Details), new { id });
            }

            booking.Status = "CheckedOut";
            booking.ActualCheckOutDate = DateTime.Now;
            booking.UpdatedAt = DateTime.Now;

            // Update room availability
            var room = await _context.Rooms.FindAsync(booking.RoomId);
            if (room != null)
            {
                room.IsAvailable = true;
                _context.Update(room);
            }

            _context.Update(booking);
            await _context.SaveChangesAsync();

            TempData["Success"] = "เช็คเอาท์สำเร็จ";
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
