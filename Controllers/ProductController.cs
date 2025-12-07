using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TufanOto.Data;
using TufanOto.Models;

namespace TufanOto.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ---------------- ÜRÜN LİSTELEME (PUBLIC) ----------------
        public IActionResult Index()
        {
            var urunler = _context.Products.ToList();
            return View(urunler);
        }

        // ---------------- ÜRÜN DETAY (PUBLIC) ----------------
        public IActionResult Detay(int id)
        {
            var urun = _context.Products.Find(id);
            if (urun == null) return NotFound();

            return View(urun);
        }

        // ---------------- ÜRÜN EKLEME (ADMIN) ----------------
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product model, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                // Resim yükleme
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
                model.IsFeatured = model.IsFeatured;

                _context.Products.Add(model);
                await _context.SaveChangesAsync();
                
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // ---------------- ÜRÜN DÜZENLEME (ADMIN) ----------------
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var urun = _context.Products.Find(id);
            if (urun == null) return NotFound();

            return View(urun);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(Product model, IFormFile? file)
        {
            if (!ModelState.IsValid)
                return View(model);

            var urun = _context.Products.Find(model.Id);
            if (urun == null) return NotFound();

            // Resim güncelleme
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

                urun.ImagePath = "/uploads/" + newImageName;
            }

            // ❗❗❗ KRİTİK SATIR — EKSİK OLAN BU
            urun.IsFeatured = model.IsFeatured;

            // Diğer alanlar
            urun.Name = model.Name;
            urun.Description = model.Description;
            urun.Stock = model.Stock;
            urun.Price = model.Price;

            _context.Products.Update(urun);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        // ---------------- ÜRÜN SİLME (ADMIN) ----------------
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var urun = _context.Products.Find(id);
            if (urun == null) return NotFound();

            return View(urun);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var urun = _context.Products.Find(id);
            if (urun == null) return NotFound();

            _context.Products.Remove(urun);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
