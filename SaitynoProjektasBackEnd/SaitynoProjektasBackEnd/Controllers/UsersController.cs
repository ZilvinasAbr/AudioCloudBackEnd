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
            var authId = _usersService.GetUserAuthId(User);

            if (authId == null)
                return BadRequest(new[] {"Invalid token provided"});

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

        [Authorize]
        [HttpPut]
        public IActionResult Put([FromBody] EditUserRequestModel user)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelStateHandler.GetModelStateErrors(ModelState);

                return BadRequest(modelErrors.ToArray());
            }

            var authId = _usersService.GetUserAuthId(User);
            if (authId == null)
                return BadRequest(new[] {"Bad access token provided"});

            var errorMessages = _usersService.EditUser(authId, user);

            if (errorMessages != null)
            {
                return NotFound(errorMessages);
            }

            return NoContent();
        }
    }
}
