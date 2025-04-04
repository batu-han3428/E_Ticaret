using E_Ticaret.Domains.Entities.Base.Models;

namespace E_Ticaret.Domains.Entities
{
    public class CartItem: BaseEntity
    {
        public Int64 CartId { get; set; }

        public Cart Cart { get; set; }

        public Int64 ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
