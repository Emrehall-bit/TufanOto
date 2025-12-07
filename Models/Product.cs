using System.ComponentModel.DataAnnotations;

namespace TufanOto.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün adı zorunludur")]
        [Display(Name = "Ürün Adı")]
        public string Name { get; set; } 

        [Display(Name = "Açıklama")]
        public string? Description { get; set; } 

        [Required]
        [Display(Name = "Fiyat")]
        public decimal Price { get; set; } 

        public int Stock { get; set; }
        public bool IsFeatured { get; set; } // fırsat ürünü mü?

        

        public string? ImagePath { get; set; } // Resim Yolu
    }
}