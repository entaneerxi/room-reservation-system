using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomReservationSystem.Data;
using RoomReservationSystem.Models;

namespace RoomReservationSystem.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Sliders = await _context.Sliders.Where(s => s.IsActive).OrderBy(s => s.DisplayOrder).ToListAsync();
        ViewBag.Promotions = await _context.Promotions.Where(p => p.IsActive && p.StartDate <= DateTime.Now && p.EndDate >= DateTime.Now).OrderByDescending(p => p.CreatedAt).Take(3).ToListAsync();
        ViewBag.Rooms = await _context.Rooms.Where(r => r.IsAvailable).OrderBy(r => r.Name).Take(6).ToListAsync();
        return View();
    }

    public async Task<IActionResult> Promotions()
    {
        var promotions = await _context.Promotions
            .Where(p => p.IsActive && p.StartDate <= DateTime.Now && p.EndDate >= DateTime.Now)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
        return View(promotions);
    }

    public async Task<IActionResult> Gallery()
    {
        var gallery = await _context.Galleries
            .Where(g => g.IsActive)
            .OrderBy(g => g.DisplayOrder)
            .ToListAsync();
        return View(gallery);
    }

    public async Task<IActionResult> Facilities()
    {
        var facilities = await _context.Facilities
            .Where(f => f.IsActive)
            .OrderBy(f => f.DisplayOrder)
            .ToListAsync();
        return View(facilities);
    }

    public async Task<IActionResult> Contact()
    {
        var contactInfo = await _context.ContactInfos.FirstOrDefaultAsync();
        return View(contactInfo);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
