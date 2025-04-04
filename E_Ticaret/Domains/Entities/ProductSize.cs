namespace E_Ticaret.Domains.Entities
{
    public class ProductSize
    {
        public long ProductId { get; set; }

        public Product Product { get; set; }

        public long SizeId { get; set; }

        public Size Size { get; set; }
    }
}
