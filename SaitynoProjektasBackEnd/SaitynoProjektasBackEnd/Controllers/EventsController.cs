using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaitynoProjektasBackEnd.Models;
using SaitynoProjektasBackEnd.Services;
using SaitynoProjektasBackEnd.Services.Interfaces;

namespace SaitynoProjektasBackEnd.Controllers
{
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        private readonly IEventsService _eventsService;
        private readonly IUsersService _usersService;

        public EventsController(IEventsService eventsService, IUsersService usersService)
        {
            _eventsService = eventsService;
            _usersService = usersService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));

            string authId = _usersService.GetUserAuthId(User);
            if (authId == null)
                return BadRequest(new []{"Invalid access token"});

            try
            {
                var events = _eventsService.GetEvents(authId);

                return Ok(events);
            }
            catch (Exception e)
            {
                return BadRequest(new[]{e.Message});
            }
        }

        [HttpGet("LastWeek")]
        public IActionResult GetLastWeek()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));

            var authId = _usersService.GetUserAuthId(User);
            if (authId == null)
                return BadRequest(new []{"Invalid access token"});

            try
            {
                var events = _eventsService.GetEventsLastWeek(authId);
                return Ok(events);
            }
            catch (Exception e)
            {
                return BadRequest(new[] { e.Message });
            }
        }
    }
}
