
using System.ComponentModel.DataAnnotations;

namespace TufanOto.Models
{
    public class CustomerRequest
    {
        [Key] // Birincil Anahtar (ID)
        public int Id { get; set; }

        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [Display(Name = "Telefon Numaranız")]
        public string PhoneNumber { get; set; } // Müşteri telefonu

        [Display(Name = "Araç Bilgisi")]
        public string? CarModel { get; set; } // Örn: 2015 Ford Focus

        [Required(ErrorMessage = "Lütfen sorunu kısaca yazınız.")]
        [Display(Name = "Sorun Açıklaması")]
        public string Description { get; set; } // "Motordan ses geliyor" vs.

        public string? ImagePath { get; set; } // Fotoğrafın yolu buraya yazılacak

        public DateTime CreatedAt { get; set; } = DateTime.Now; // Ne zaman gönderdi?

        public bool IsResolved { get; set; } = false; // Usta baktı mı?
    }
}