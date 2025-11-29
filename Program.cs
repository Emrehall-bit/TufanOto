using Microsoft.EntityFrameworkCore;
using TufanOto.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabaný Baðlantýsý (Mutlaka 'var app' satýrýndan ÖNCE olmalý)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. MVC Servisleri
builder.Services.AddControllersWithViews();

// --- ÝNÞA ETME AÞAMASI (Buradan sonra servis eklenemez) ---
var app = builder.Build();

// Hata yönetimi ve HTTPS ayarlarý
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();