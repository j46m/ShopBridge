using System.ComponentModel.DataAnnotations;

namespace ShopBridge.Domain.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ProductCode { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [MaxLength(3000)]
        public string Description { get; set; }

        [Required]
        [Range(0.0, double.MaxValue)]
        public decimal Price { get; set; } = 0;

        [Required]
        [Range(0, uint.MaxValue, ErrorMessage = "Quantity has to be greater or equal to one.")]
        public uint Quantity { get; set; }
    }
}
