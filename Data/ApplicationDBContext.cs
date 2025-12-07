using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TufanOto.Models;
 
namespace TufanOto.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // CustomerRequest modelini veritabanında tabloya dönüştür
        public DbSet<CustomerRequest> CustomerRequests { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}