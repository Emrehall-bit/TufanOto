using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TufanOto.Data;
using TufanOto.Models;

namespace TufanOto.Controllers
{
    [Authorize(Roles = "Admin")] // SADECE ADMIN GİREBİLİR
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // === ADMIN PANEL ANA SAYFASI ===
        public IActionResult Index()
        {
            var talepler = _context.CustomerRequests
                                   .OrderByDescending(x => x.CreatedAt)
                                   .ToList();

            // Admin Layout kullan
            ViewData["Layout"] = "_AdminLayout";
            return View(talepler);
        }
    }
}
