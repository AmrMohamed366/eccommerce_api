using eccommerce_api.Data;
using eccommerce_api.model;
using eccommerce_api.ModelDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eccommerce_api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CategoryController : Controller
    {
        public CategoryController(Application _context)
        {
            Context = _context;
        }

        public Application Context { get; }
        [HttpPost("Add")]
        public async Task<ActionResult> addCategory(CategoryDto dto)
        {
            var cate = new Category
            {
                Name = dto.Name,
            };

            if (dto.Name == null) return BadRequest(new {message= "Enter category name"});

            await Context.categories.AddAsync(cate);
            await Context.SaveChangesAsync();
            return Ok(new
            {
                message= "success",
                data= new
                {
                    cate.Id,
                    cate.Name
                }
            });

        }



        [HttpGet("Get")]

        public async Task <ActionResult> getCategory([FromQuery] int pageNumber= 1, [FromQuery]int pageSize = 10)
        {
            var recoder = await Context.categories.ToListAsync();

            return Ok(new
            {
                mesage = "Success",
                data = 

                    recoder.Skip((pageNumber-1)* pageSize).Take(pageSize).Select(c => new
                    {
                        c.Name,
                        c.Id
                    }).ToList(),
                    count = recoder.Count,
                    pageSize =pageSize,
                    pageNumber=pageNumber
            });
        }
    }
}
