using E_Ticaret.Domains.Entities.Base.Models;

namespace E_Ticaret.Domains.Entities
{
    public class Cart: BaseEntity
    {
        public Int64? UserId { get; set; }

        public Guid? AnonymousId { get; set; }

        public List<CartItem> CartItems { get; set; }
    }
}
