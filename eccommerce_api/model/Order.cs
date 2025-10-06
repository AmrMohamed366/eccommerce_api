namespace eccommerce_api.model
{
    public class Order
    {

        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ProductId { get; set; }

        public List<ProductItem>? Products { get; set; }
        public int UserId { get; set; }

        public List<User> ?Users { get; set; }
    }
}
