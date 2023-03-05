using System.ComponentModel.DataAnnotations;

namespace ShopBridge.ViewModels
{
    public class ProductViewModel
    {
        public string ProductCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; } = 0;
        public uint Quantity { get; set; }
    }
}
