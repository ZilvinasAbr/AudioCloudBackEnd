using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaitynoProjektasBackEnd.Models;
using SaitynoProjektasBackEnd.RequestModels;
using SaitynoProjektasBackEnd.Services;

namespace SaitynoProjektasBackEnd.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var users = _usersService.GetUsers();

            return Ok(users);
        }

        [Authorize]
        [HttpGet("claims")]
        public object Claims()
        {
            return User.Claims.Select(c =>
                new
                {
                    Type = c.Type,
                    Value = c.Value
                });
        }

        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            var user = _usersService.GetUserByName(name);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPut("{name}")]
        public IActionResult Put(string name, [FromBody]EditUserRequestModel user)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelStateHandler.GetModelStateErrors(ModelState);

                return BadRequest(modelErrors.ToArray());
            }

            var errorMessages = _usersService.EditUser(name, user);

            if (errorMessages != null)
            {
                return NotFound(errorMessages);
            }

            return NoContent();
        }
    }
}
