using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TufanOto.Data;   // Veritabaný baðlantýsý
using TufanOto.Models; // Tablo yapýsý

namespace TufanOto.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; // Veritabaný eriþim aracý

        // Constructor: Veritabanýný buraya baðlýyoruz
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet]
        public IActionResult Teklif()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }

        // --- ÝÞTE SÝHRÝN OLDUÐU YER: FORM BURAYA GELECEK ---
        [HttpPost]
        [ValidateAntiForgeryToken] // Güvenlik kilidi
        public async Task<IActionResult> TeklifAl(CustomerRequest model, IFormFile? file)
        {
            if (ModelState.IsValid) // Veriler kurallara uygun mu? (Tel boþ deðil vs.)
            {
                // 1. Resim Yükleme Ýþlemi
                if (file != null && file.Length > 0)
                {
                    // Dosya uzantýsýný al (.jpg)
                    var extension = Path.GetExtension(file.FileName);
                    // Rastgele isim ver (resim-543543.jpg) ki çakýþmasýn
                    var newImageName = Guid.NewGuid() + extension;

                    // Klasör yoksa oluþtur
                    var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

                    // Resmi kaydet
                    var location = Path.Combine(uploadDir, newImageName);
                    using (var stream = new FileStream(location, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Veritabanýna resmin yolunu yaz
                    model.ImagePath = "/uploads/" + newImageName;
                }

                // 2. Veritabanýna Kaydetme
                _context.CustomerRequests.Add(model);
                await _context.SaveChangesAsync(); // SQL'e "Insert" komutunu yollar

                // Baþarýlý mesajý (TempData sayfalar arasý taþýnýr)
                TempData["Message"] = "Talebini aldýk! Ustamýz en kýsa sürede dönüþ yapacak.";

                // Sayfayý yenile (Ana sayfaya git)
                return RedirectToAction("Teklif");
            }

            // Bir hata varsa formu geri gönder (Hatalarý görsün)
            return View("Teklif", model);
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
}