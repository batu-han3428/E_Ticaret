namespace Common.Model
{
    public class ProductModel
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }

        public int StockQuantity { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }
    }
}
