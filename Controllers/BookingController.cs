using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomReservationSystem.Data;
using RoomReservationSystem.Models;
using RoomReservationSystem.ViewModels;

namespace RoomReservationSystem.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var rooms = await _context.Rooms
                .Where(r => r.IsAvailable)
                .OrderBy(r => r.Name)
                .ToListAsync();
            return View(rooms);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null || !room.IsAvailable)
            {
                return NotFound();
            }

            var model = new BookingViewModel
            {
                RoomId = roomId,
                Room = room,
                CheckInDate = DateTime.Today.AddDays(1),
                CheckOutDate = DateTime.Today.AddDays(2),
                AvailableOptions = await _context.AdditionalOptions.Where(a => a.IsActive).ToListAsync()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var room = await _context.Rooms.FindAsync(model.RoomId);

                if (user == null || room == null)
                {
                    return NotFound();
                }

                // Calculate total amount
                var days = (model.CheckOutDate - model.CheckInDate).Days;
                var totalAmount = model.BookingType == "Daily" 
                    ? room.DailyRate * days 
                    : room.MonthlyRate;

                // Apply promotion if promo code provided
                decimal? discountAmount = null;
                int? promotionId = null;

                if (!string.IsNullOrEmpty(model.PromoCode))
                {
                    var promotion = await _context.Promotions
                        .FirstOrDefaultAsync(p => p.PromoCode == model.PromoCode 
                            && p.IsActive 
                            && p.StartDate <= DateTime.Now 
                            && p.EndDate >= DateTime.Now);

                    if (promotion != null)
                    {
                        promotionId = promotion.Id;
                        discountAmount = promotion.DiscountAmount ?? (totalAmount * promotion.DiscountPercentage / 100);
                        totalAmount -= discountAmount.Value;
                    }
                }

                var booking = new Booking
                {
                    UserId = user.Id,
                    RoomId = model.RoomId,
                    CheckInDate = model.CheckInDate,
                    CheckOutDate = model.CheckOutDate,
                    BookingType = model.BookingType,
                    TotalAmount = totalAmount,
                    PromotionId = promotionId,
                    DiscountAmount = discountAmount,
                    Notes = model.Notes,
                    Status = "Pending",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(MyBookings));
            }

            model.Room = await _context.Rooms.FindAsync(model.RoomId);
            model.AvailableOptions = await _context.AdditionalOptions.Where(a => a.IsActive).ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> MyBookings()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var bookings = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.Promotion)
                .Where(b => b.UserId == user.Id)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(bookings);
        }

        [HttpGet]
        public async Task<IActionResult> Reschedule(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || booking.UserId != user.Id)
            {
                return Forbid();
            }

            if (booking.Status != "Pending" && booking.Status != "Confirmed")
            {
                TempData["Error"] = "ไม่สามารถเลื่อนการจองนี้ได้";
                return RedirectToAction(nameof(MyBookings));
            }

            var model = new BookingViewModel
            {
                RoomId = booking.RoomId,
                Room = booking.Room,
                CheckInDate = booking.CheckInDate,
                CheckOutDate = booking.CheckOutDate,
                BookingType = booking.BookingType,
                Notes = booking.Notes
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reschedule(int id, BookingViewModel model)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || booking.UserId != user.Id)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                var room = await _context.Rooms.FindAsync(model.RoomId);
                if (room == null)
                {
                    return NotFound();
                }

                // Calculate new total amount
                var days = (model.CheckOutDate - model.CheckInDate).Days;
                var totalAmount = model.BookingType == "Daily" 
                    ? room.DailyRate * days 
                    : room.MonthlyRate;

                booking.CheckInDate = model.CheckInDate;
                booking.CheckOutDate = model.CheckOutDate;
                booking.TotalAmount = totalAmount;
                booking.Notes = model.Notes;
                booking.UpdatedAt = DateTime.Now;

                _context.Update(booking);
                await _context.SaveChangesAsync();

                TempData["Success"] = "เลื่อนการจองสำเร็จ";
                return RedirectToAction(nameof(MyBookings));
            }

            model.Room = await _context.Rooms.FindAsync(model.RoomId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || booking.UserId != user.Id)
            {
                return Forbid();
            }

            if (booking.Status == "CheckedIn" || booking.Status == "CheckedOut")
            {
                TempData["Error"] = "ไม่สามารถยกเลิกการจองนี้ได้";
                return RedirectToAction(nameof(MyBookings));
            }

            booking.Status = "Cancelled";
            booking.UpdatedAt = DateTime.Now;

            _context.Update(booking);
            await _context.SaveChangesAsync();

            TempData["Success"] = "ยกเลิกการจองสำเร็จ";
            return RedirectToAction(nameof(MyBookings));
        }
    }
}
