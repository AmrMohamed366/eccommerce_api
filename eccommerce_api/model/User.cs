namespace eccommerce_api.model
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }


        public int? OrderId { get; set; }
        public Order? Orders { get; set; }

        

    }
}
