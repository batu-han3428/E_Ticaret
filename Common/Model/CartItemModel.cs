namespace Common.Model
{
    public class CartItemModel
    {
        public Int64 Id { get; set; }
        public Int64 CartId { get; set; }
        public CartModel Cart { get; set; }

        public Int64 ProductId { get; set; }
        public ProductModel Product { get; set; }

        public int Quantity { get; set; }
    }
}
