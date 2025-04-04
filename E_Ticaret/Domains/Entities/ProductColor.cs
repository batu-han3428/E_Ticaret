namespace E_Ticaret.Domains.Entities
{
    public class ProductColor
    {
        public long ProductId { get; set; }

        public Product Product { get; set; }

        public long ColorId { get; set; }

        public Color Color { get; set; }
    }
}
