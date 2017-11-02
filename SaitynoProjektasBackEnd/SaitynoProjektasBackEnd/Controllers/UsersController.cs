using System;
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
        [HttpGet("Current")]
        public IActionResult GetCurrentUser()
        {
            try
            {
                var user = _usersService.GetCurrentUser(User);
                return Ok(user);    
            } catch (Exception e)
            {
                return BadRequest(new[] {e.Message});
            }
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

            try
            {
                var user = _usersService.RegisterUser(authId);

                return Created($"api/users/${user.UserName}", user.UserName);
            }
            catch (Exception e)
            {
                return BadRequest(new[] {e.Message});
            }
        }

        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));

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
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));

            var authId = _usersService.GetUserAuthId(User);
            if (authId == null)
                return BadRequest(new[] {"Bad access token provided"});

            try
            {
                _usersService.EditUser(authId, user);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(new[] { e.Message });
            }
        }

        [HttpGet("{userName}/Followings")]
        public IActionResult GetFollowings(string userName)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));
            
            try {
                var followings = _usersService.GetUserFollowings(userName);

                return Ok(followings);
            } catch (Exception e)
            {
                return BadRequest(new[] { e.Message });
            }
        }

        [HttpGet("Test")]
        public IActionResult Test()
        {
            return Ok("Test");
        }
    }
}
