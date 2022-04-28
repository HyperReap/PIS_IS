using Hotel_PIS.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Hotel_PIS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(Employee user)
        {
            CreatePasswordHash(user.Password, out byte[] passwordHash);

            using (var db = new HotelContext())
            {
                user.Password = Encoding.UTF8.GetString(passwordHash);
                db.Employees.Add(user);
                db.SaveChanges();

                return Ok(user);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto user)
        {

            using (var db = new HotelContext())
            {
                var emplyee = db.Employees.Where(x => x.Email == user.Email).FirstOrDefault();
                if (emplyee == null)
                    return BadRequest("User not found");

                using (var hmac = new HMACSHA512())
                {
                    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(user.Password));
                    return BadRequest(Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(emplyee.Password)));
                }

                if (!VerifyPasswordHash(user.Password, Encoding.UTF8.GetBytes(emplyee.Password)))
                {
                    return BadRequest(emplyee.Password);
                }
            }            

            string token = CreateToken(user);
            return Ok(token);
        }

        private string CreateToken(UserDto user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                 new Claim(ClaimTypes.Role, "Manager")

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);
        
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash)
        {
            return true;
            using (var hmac = new HMACSHA512())
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
