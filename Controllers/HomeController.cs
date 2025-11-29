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
        // --- GÜNCELLENMÝÞ TEKLÝF AL METODU ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TeklifAl(CustomerRequest model, IFormFile? file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // 1. Resim Yükleme Ýþlemi
                    if (file != null && file.Length > 0)
                    {
                        var extension = Path.GetExtension(file.FileName);
                        var newImageName = Guid.NewGuid() + extension;
                        var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

                        if (!Directory.Exists(uploadDir)) Directory.CreateDirectory(uploadDir);

                        var location = Path.Combine(uploadDir, newImageName);
                        using (var stream = new FileStream(location, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        model.ImagePath = "/uploads/" + newImageName;
                    }

                    // 2. Veritabanýna Kaydetme
                    _context.CustomerRequests.Add(model);
                    await _context.SaveChangesAsync();

                    // BAÞARILI MESAJI
                    TempData["Message"] = "Talebini aldýk! Ustamýz en kýsa sürede dönüþ yapacak.";

                    // Ýstersen Ana Sayfaya ("Index"), Ýstersen Teklif Sayfasýna ("Teklif") yönlendir.
                    // Kullanýcý formun olduðu yerde kalsýn mesajý görsün diyorsan "Teklif" daha iyidir.
                    return RedirectToAction("Teklif");
                }
            }
            catch (Exception ex)
            {
                // HATA OLURSA BURAYA DÜÞECEK
                // Hatayý kullanýcýya gösterelim ki nedenini anlayalým (Normalde kullanýcýya ex.Message gösterilmez ama geliþtirme aþamasýndayýz)
                TempData["Error"] = "Bir hata oluþtu: " + ex.Message;
            }

            // Hata varsa veya form eksikse sayfayý yenileme, olduðu gibi göster
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