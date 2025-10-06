using eccommerce_api.Data;
using eccommerce_api.model;
using eccommerce_api.ModelDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace eccommerce_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        public OrderController(Application _context)
        {
            Context = _context;
        }

        public Application Context { get; }

        [HttpPost("Add")]
        [Authorize(Roles ="User")]
        public async Task<ActionResult> CreateOrder(List<OrderDto> items) {


            var UserEmail = User.FindFirst(ClaimTypes.Email).Value;
            if (UserEmail == null) return Unauthorized();


            var user = await Context.Users.FirstOrDefaultAsync(u => u.Email == UserEmail);
            if(user == null) return Unauthorized();

            var order = new Order
            {
                UserId = user.Id,
                CreatedDate = DateTime.UtcNow,
                Products =new List<ProductItem>()
            };

            foreach(var item in items)
            {
                var product= await Context.products.FirstOrDefaultAsync(p=>p.Id == item.ProductId);
                if(product == null) BadRequest(new {message=$" this prouct  NotFound"});

                order.Products.Add(new ProductItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    

                });


            }
            await Context.Orders.AddAsync(order);
            await Context.SaveChangesAsync();
            return Ok(new
            {
                success = true,
                message = "Success",
                data = new
                {
                    order.Id
                }
            });

        }

        [HttpGet("Get")]
        [Authorize(Roles ="User")]
        public async Task<ActionResult> getOrders([FromQuery] int pageNumber = 1, [FromQuery]int pagesize=10)
        {
            var useremail = User.FindFirst(ClaimTypes.Email).Value;
            if(useremail == null)return Unauthorized();

          var orders=   await Context.Orders.Include(o => o.Products).Skip((pageNumber-1)*pagesize).Take(pagesize).Select(o => new
            {
              o.CreatedDate,
              o.Id,
              items= 
                  o.Products.Select(p => new
                  {
                      p.ProductId,
                      p.Quantity,

                      
                  })
              
            }).ToListAsync();

            return Ok(new
            {
                message = "Success",
                data = orders
            });

        }

    }
}
