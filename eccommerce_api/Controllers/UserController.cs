using Azure.Core;
using eccommerce_api.Data;
using eccommerce_api.model;
using eccommerce_api.ModelDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Text;

namespace eccommerce_api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public UserController(Application _context,JwtOption _option )
        {
            Context = _context;
            Option = _option;
        }

        public Application Context { get; }
        public JwtOption Option { get; }

        [HttpPost("Register")]

        public async Task<ActionResult> register(RegisterDto dto)
        {

            var user= Context.Users.FirstOrDefault(u=> u.Email== dto.Email);
            if (user != null) return BadRequest(new
            {
                message = "This Email Used before"
            });

            var newUser = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Password = dto.Password,
            };
            await Context.Users.AddAsync(newUser);

            await Context.SaveChangesAsync();

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDespictor = new SecurityTokenDescriptor
            {
               Subject= new ClaimsIdentity(new[]
               {
                   new Claim (ClaimTypes.NameIdentifier, newUser.Id.ToString()),
                   new Claim(ClaimTypes.Name, newUser.Name),
                   new Claim(ClaimTypes.Email, newUser.Email),
                   new Claim(ClaimTypes.Role, "User")
               }),
               Issuer= Option.Issuer,
               Audience= Option.Audience,
               Expires= DateTime.UtcNow.AddDays(1),
               SigningCredentials= new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Option.SigningKey)), SecurityAlgorithms.HmacSha256)

            };

            var accessToken= tokenHandler.CreateToken(tokenDespictor);
            var token = tokenHandler.WriteToken(accessToken);
            return Ok(
                new
                {
                    message= "You Rigister Now",
                    User= new
                    {
                        newUser.Id,
                        newUser.Name,
                        newUser.Email,
                        newUser.Phone
                    },
                    token=token,
                });

        }
     [HttpPost("Login")]

        public async Task<ActionResult> Login(LoginDto dto)
        {

            var user= Context.Users.FirstOrDefault(u=> u.Email== dto.Email);
            if (user == null) return BadRequest(new
            {
                message = "This Email not found"
            });

            user.Email = dto.Email;
            user.Password= dto.Password;

         

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDespictor = new SecurityTokenDescriptor
            {
               Subject= new ClaimsIdentity(new[]
               {
                   new Claim (ClaimTypes.NameIdentifier, user.Id.ToString()),
                   new Claim(ClaimTypes.Name, user.Name),
                   new Claim(ClaimTypes.Email, user.Email),
                   new Claim(ClaimTypes.Role, "User")
               }),
               Issuer= Option.Issuer,
               Audience= Option.Audience,
               Expires= DateTime.UtcNow.AddDays(1),
               SigningCredentials= new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Option.SigningKey)), SecurityAlgorithms.HmacSha256)

            };

            var accessToken= tokenHandler.CreateToken(tokenDespictor);
            var token = tokenHandler.WriteToken(accessToken);
            return Ok(
                new
                {
                    message= "Success",
                    User= new
                    {
                        user.Id,
                        user.Name,
                        user.Email,
                        user.Phone
                    },
                    token=token,
                });

        }
   
    }
}
