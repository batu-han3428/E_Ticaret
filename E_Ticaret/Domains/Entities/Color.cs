using E_Ticaret.Domains.Entities.Base.Models;

namespace E_Ticaret.Domains.Entities
{
    public class Color: BaseEntity
    {
        public string Name { get; set; }

        public ICollection<ProductColor> ProductColors { get; set; } = new List<ProductColor>();
    }
}
