using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkWithCoreAPI.Entities;
using EntityFrameworkWithCoreAPI.Tools;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EntityFrameworkWithCoreAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace EntityFrameworkWithCoreAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ProductDBContext _context;
        private readonly IConfiguration _config;

        public UsersController(ProductDBContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> login([FromBodyAttribute] Login userLogin)
        {
            string password = Password.hashPassword(userLogin.Password);
            var dbUser = await _context.User.Where(u => u.Email == userLogin.Email && u.Password == password).FirstOrDefaultAsync();

            if (dbUser != null)
            {
                //generate jwt token and return it to the user
                var token = GenerateToken(dbUser);
                return Ok(token);
            }
            else
            {
                return BadRequest("Username or password is incorrect.");
            }
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBodyAttribute] User user)
        {
            var dbUser = _context.User.Where(u => u.Email == user.Email).FirstOrDefault();

            if (dbUser != null)
            {
                return BadRequest("This email has already been registered.");
            }

            user.Password = Password.hashPassword(user.Password);

            _context.User.Add(user);
            _context.SaveChangesAsync();

            return Ok("User has been successfully registered.");
        }

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Email),
                new Claim(ClaimTypes.Role,user.Role)
            };
            var token = new JwtSecurityToken(_config["JwtSettings:Issuer"],
                _config["JwtSettings:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
