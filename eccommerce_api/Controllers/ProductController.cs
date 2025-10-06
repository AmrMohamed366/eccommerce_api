using eccommerce_api.Data;
using eccommerce_api.model;
using eccommerce_api.ModelDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eccommerce_api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly Application context;

        public ProductController(Application context)
        {
            this.context = context;
        }

        [HttpPost("AddProduct")]
        public async Task<ActionResult> addProduct(ProductDto? dto)
        {
            var product = new Products
            {

                Description = dto.Description,
                Name = dto.Name,
                Price = dto.Price,
                OldPrice = dto.OldPrice,
                CategoryId= dto.CategopryId
            };

            if (dto.Name == null || dto.Price == 0|| dto.CategopryId==0) return BadRequest(new { message = "Enter product Data name, price" });

            await context.products.AddAsync(product);
            await context.SaveChangesAsync();
            return Ok(new
            {
                message= "Success",
                data = new
                {
                    id = product.Id,
                    price = product.Price,
                    name = product.Name,
                    desc = product.Description,
                    oldPrice = product.OldPrice,
                }
            });
        }


        [HttpGet("Get")]
        public async Task<IActionResult> getProduct()
        {

            var product = await context.products.ToListAsync();
            return Ok(new
            {
                message= "success",
                data= product
            });

        }


        [HttpPut("Update")]

        public async Task<ActionResult> Update(ProductDto dto, int id)
        {
            var product = await context.products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return BadRequest();

            product.Name= dto.Name;
            product.Price= dto.Price;
            product.OldPrice= dto.OldPrice;
            product.Description= dto.Description;
             context.products.Update
                (product);
            await context.SaveChangesAsync();
            return Ok(new
            {
                sucsses = true,
                message = "sucsses",
                data
                = product
            });
        }


        [HttpGet("Delete")]

        public async Task <ActionResult> delete(int id)
        {
            var product = await context.products.FirstOrDefaultAsync(p => p.Id == id);
            if(product == null) return BadRequest();

            await context.SaveChangesAsync();
            return Ok(new
            {
                sucsses = true,
                message = "you deleted thid product"
            });

        }

        [HttpGet("Filter")]

       public async Task<IActionResult> filter([FromQuery] int? catyegoryId, [FromQuery] decimal? maxPrice, [FromQuery] string ?name, [FromQuery]  decimal ?minPrice, [FromQuery] int pageNumber= 1, [FromQuery]int pageSize=10)
       {
            IQueryable<Products> query = context.products.Include(p => p.Category);

          
            if (!string.IsNullOrEmpty(name))
            {
             
                query= query.Where(p=>p.Name.Contains(name)|| p.Description.Contains(name));
            }
          
            if(catyegoryId .HasValue)
            {
                query= query.Where(p=>p.CategoryId==catyegoryId);
            }
            if(maxPrice .HasValue&& maxPrice>0)
            {
                query = query.Where(p=> p.Price<maxPrice);
            }
            if (minPrice.HasValue&& minPrice>0)
            {
                query=  query.Where(p=>p.Price>minPrice);
            }
            if (minPrice > 0 && maxPrice > 0 && minPrice > maxPrice)
            {
                return BadRequest(new
                {
                    message = "minPrice must not be greater than maxPrice"
                });
            }
           
            return Ok(new
            {
                message= "success",
                data =await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).Select(p => new
                {
                    p.Id,
                    p.CategoryId,
                    p.Description,
                    p.Price,
                    p.Name,
                    p.OldPrice,
                    category = new
                    {
                        p.Category.Name,
                        p.Category.Id
                    }
                }).ToListAsync()
        });
        }

    }
}
