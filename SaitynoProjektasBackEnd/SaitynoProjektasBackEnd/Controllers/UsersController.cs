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
        [HttpGet("Claims")]
        public object Claims()
        {
            return User.Claims.Select(c =>
                new
                {
                    c.Type,
                    c.Value
                });
        }

        [Authorize]
        [HttpPost("Register")]
        public IActionResult Register()
        {
            var type = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

            var claims = User.Claims.Select(c =>
                new
                {
                    c.Type,
                    c.Value
                });

            var authId = claims.SingleOrDefault(c => c.Type == type)?.Value;

            if (authId == null)
                return BadRequest("Invalid token provided");

            var errorMessages = _usersService.RegisterUser(authId);

            if (errorMessages != null)
                return BadRequest(errorMessages);

            return NoContent();
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
        public IActionResult Put(string name, [FromBody] EditUserRequestModel user)
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
