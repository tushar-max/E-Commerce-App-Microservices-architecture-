using AuthAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private List<User> _users = new List<User>
        {
            new User { userName = "user1", password = "password1", role = "User" },
            new User { userName = "user2", password = "password2", role = "Admin" }
        };
        public AuthController()
        {
            
        }

        [HttpPost]
        public IActionResult LoginUser([FromBody] UserDTO loginuser)
        {
            var loginResponse = _users.Find(u => u.userName == loginuser.userName && u.password == loginuser.password);
            if (loginResponse == null)
            {
                return Unauthorized($"Login Failed");
            }
            var token = CreateJwt(loginResponse);
            return Ok(new
            {
                Token = token,
                Login = true
            });
        }

        private string CreateJwt(User login)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("this is my custom Secret key for authentication");
            var identity = new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.Role,login.role),
            new Claim(ClaimTypes.Name,login.userName),
            });
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
