using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkWithCoreAPI.Entities;
using EntityFrameworkWithCoreAPI.Tools;

namespace EntityFrameworkWithCoreAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ProductDBContext _context;

        public UsersController(ProductDBContext context)
        {
            _context = context;
        }
     

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> login([FromBodyAttribute] User user)
        {
            string password = Password.hashPassword(user.Password);
            var dbUser = await _context.User.Where(u => u.Email == user.Email && u.Password == password).FirstOrDefaultAsync();

            if (dbUser != null)
            {
                return Ok(dbUser);
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
    }
}
