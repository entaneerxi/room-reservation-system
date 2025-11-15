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
    public class UtilityBillsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UtilityBillsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var bills = await _context.UtilityBills
                .Include(u => u.MonthlyRental)
                    .ThenInclude(m => m!.User)
                .Include(u => u.MonthlyRental)
                    .ThenInclude(m => m!.Room)
                .OrderByDescending(u => u.Year)
                .ThenByDescending(u => u.Month)
                .ToListAsync();
            return View(bills);
        }

        public IActionResult Create()
        {
            ViewData["MonthlyRentalId"] = new SelectList(
                _context.MonthlyRentals
                    .Include(m => m.User)
                    .Include(m => m.Room)
                    .Where(m => m.Status == "Active")
                    .Select(m => new { 
                        m.Id, 
                        DisplayText = m.User!.FirstName + " " + m.User.LastName + " - " + m.Room!.Name 
                    }),
                "Id", 
                "DisplayText");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UtilityBill bill)
        {
            if (ModelState.IsValid)
            {
                bill.TotalAmount = bill.WaterAmount + bill.ElectricityAmount;
                bill.CreatedAt = DateTime.Now;
                _context.Add(bill);
                await _context.SaveChangesAsync();
                TempData["Success"] = "บันทึกค่าน้ำค่าไฟสำเร็จ";
                return RedirectToAction(nameof(Index));
            }
            ViewData["MonthlyRentalId"] = new SelectList(_context.MonthlyRentals, "Id", "Id", bill.MonthlyRentalId);
            return View(bill);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsPaid(int id)
        {
            var bill = await _context.UtilityBills.FindAsync(id);
            if (bill != null)
            {
                bill.Status = "Paid";
                bill.PaidDate = DateTime.Now;
                _context.Update(bill);
                await _context.SaveChangesAsync();
                TempData["Success"] = "ยืนยันการชำระเงินสำเร็จ";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
