namespace eccommerce_api.model
{
    public class ProductItem
    {

      public Guid Id { get; set; }
        public decimal Quantity { get; set; }

        public int ProductId {  get; set; }
        List<Products>? Products { get; set; }


    }
}
