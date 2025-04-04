using E_Ticaret.Domains.Entities.Base.Models;

namespace E_Ticaret.Domains.Entities
{
    public class Size: BaseEntity
    {
        public string Name { get; set; }

        public ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
    }
}
