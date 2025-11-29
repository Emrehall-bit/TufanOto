using Microsoft.AspNetCore.Mvc;
using TufanOto.Data;   // Veritabanı
using TufanOto.Models; // Tablo

namespace TufanOto.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Admin Ana Sayfası: Gelen Talepleri Listele
        public IActionResult Index()
        {
            // Veritabanından talepleri çek, tarihe göre tersten sırala (En yeni en üstte)
            var talepler = _context.CustomerRequests
                                   .OrderByDescending(x => x.CreatedAt)
                                   .ToList();

            return View(talepler);
        }
    }
}
