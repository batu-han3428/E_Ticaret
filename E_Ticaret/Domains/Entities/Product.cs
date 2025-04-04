using E_Ticaret.Domains.Entities.Base.Models;

namespace E_Ticaret.Domains.Entities
{
    public class Product: BaseEntity
    {
        public string Name { get; set; }

        public int StockQuantity { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

        public ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();

        public ICollection<ProductColor> ProductColors { get; set; } = new List<ProductColor>();
    }
}
