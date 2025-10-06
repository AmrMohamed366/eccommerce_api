namespace eccommerce_api.ModelDto
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public int CategopryId { get; set; }
    }
}
