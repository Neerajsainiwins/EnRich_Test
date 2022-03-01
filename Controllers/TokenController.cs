using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Enrich.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Enrich.DTO;
using System.Configuration;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace Enrich.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configurations;
        private readonly userdbContext _context;
        public TokenController(IConfiguration configuration, userdbContext context)
        {
            _configurations = configuration;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserInfo userInfo)
        {
            if (userInfo != null)
            {
                var userDetail = await GetUserDetail(userInfo.Email, userInfo.Password);
                if (userDetail != null)
                {
                    var claims = new[]
                    {
                        new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, _configurations["Jwt:Subject"]),
                        new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Id", userDetail.Id.ToString()),
                        new Claim("FirstName", userDetail.FirstName),
                        new Claim("LastName", userDetail.LastName),
                        new Claim("Email", userDetail.Email),
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurations["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(_configurations["Jwt:Issuer"], _configurations["Jwt:Issuer"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<Users> GetUserDetail(string email, string password)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
