using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopBridge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _config;
        public record AuthenticationData(string? UserName, string? Password);
        public record UserData(int Id, string FirstName, string LastName, string UserName);

        public AuthenticationController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("token")]
        [AllowAnonymous]
        public ActionResult<string> Authenticate([FromBody] AuthenticationData data)
        {
            UserData? user;

            try
            {
                user = ValidateCredentials(data);
            }
            catch (Exception)
            {
                return StatusCode(500, "Error validating user's credentials");
            }


            if (user is null)
            {
                return Unauthorized();  
            }

            string token;

            try
            {
                token = GenerateToken(user);
            }
            catch 
            {
                return StatusCode(500, "Error generating user's token");
            }
            

            return Ok(token);
        }

        private UserData? ValidateCredentials(AuthenticationData data)
        {
            // Replace with a call to AUTH0, Azure or Database

            if (CompareValues(data.UserName, "usernamedemo") && CompareValues(data.Password, "userpassworddemo"))
            {
                return new UserData(1, "userdemo", "userpassworddemo", "usernamedemo");
            }

            return null;
        }

        private bool CompareValues(string? actual, string expected)
        {
            if (actual is not null)
            {
                if (actual.Equals(expected))
                {
                    return true;
                }
            }

            return false;
        }

        private string GenerateToken(UserData user)
        {
            var key = Encoding.ASCII.GetBytes(_config.GetValue<string>("Authentication:SecretKey"));
            var issuer = _config.GetValue<string>("Authentication:Issuer");
            var audience = _config.GetValue<string>("Authentication:audience");

            var secretKey = new SymmetricSecurityKey(key);

            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new()
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new(JwtRegisteredClaimNames.GivenName, user.FirstName),
                new(JwtRegisteredClaimNames.FamilyName, user.LastName)
            };

            var token = new JwtSecurityToken(issuer,
                                             audience,
                                             claims,
                                             DateTime.UtcNow,
                                             DateTime.UtcNow.AddMinutes(10),
                                             signinCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
