namespace Common.Model
{
    public class CartModel
    {
        public Int64 Id { get; set; }

        public List<CartItemModel> CartItems { get; set; }
    }
}
