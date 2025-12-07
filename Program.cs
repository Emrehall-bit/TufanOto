using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore; // SQL Server için gerekli
using System.IO;
using TufanOto.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = Directory.GetCurrentDirectory() // Klasör yolunu garantiye alýr
});

// 1. Veritabaný Baðlantýsý (Mutlaka 'var app' satýrýndan ÖNCE olmalý)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Giriþ yapmamýþsa buraya at
        options.ExpireTimeSpan = TimeSpan.FromDays(30); // Beni hatýrla süresi
    });
// 2. MVC Servisleri
builder.Services.AddControllersWithViews();
var app = builder.Build();
// --- ÝNÞA ETME AÞAMASI (Buradan sonra servis eklenemez) ---


// Hata yönetimi ve HTTPS ayarlarý
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();