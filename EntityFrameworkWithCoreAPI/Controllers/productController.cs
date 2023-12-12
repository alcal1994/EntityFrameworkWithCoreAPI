using EntityFrameworkWithCoreAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EntityFrameworkWithCoreAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class productController : ControllerBase
    {

        private readonly ProductDBContext _context;

        public productController(ProductDBContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet(Name = "GetProduct")]
        public List<Product> Get()
        {
                //seed database with records
                /*
                db.Products.AddRange(new Product
                {
                    Name = "MSI Laptop",
                    Description = "Gaming Laptop",
                    Price = 1499.99
                },
                new Product
                {
                    Name = "Logitech Keybaord",
                    Description = "Gaming Keyboard",
                    Price = 99.99
                });

                //save database changes
                db.SaveChanges();
                */

                //return table records from products table
                List<Product> list = _context.Products.ToList();

            return list;
        }
    }
}
