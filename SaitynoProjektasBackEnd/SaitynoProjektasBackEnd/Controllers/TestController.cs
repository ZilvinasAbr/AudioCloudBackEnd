using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaitynoProjektasBackEnd.Data;

namespace SaitynoProjektasBackEnd.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private readonly ApplicationDbContext _dbContext;

        public TestController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpDelete]
        public IActionResult CleanDatabase()
        {
            try
            {
                DbInitializer.CleanDatabase(_dbContext);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new[] {e.Message});
            }
        }

        [HttpPost]
        public IActionResult SeedDatabase()
        {
            DbInitializer.Initialize(_dbContext);
            return Ok();
        }
    }
}
