using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaitynoProjektasBackEnd.Models;
using SaitynoProjektasBackEnd.Services;

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
            {
                var modelErrors = ModelStateHandler.GetModelStateErrors(ModelState);

                return BadRequest(modelErrors.ToArray());
            }

            string authId = _usersService.GetUserAuthId(User);
            if (authId == null)
                return BadRequest(new []{"Invalid access token"});

            var errorMessages = _eventsService.GetEvents(authId, out var events);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

            return Ok(events);
        }

        [HttpGet("LastWeek")]
        public IActionResult GetLastWeek()
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelStateHandler.GetModelStateErrors(ModelState);

                return BadRequest(modelErrors.ToArray());
            }

            string authId = _usersService.GetUserAuthId(User);
            if (authId == null)
                return BadRequest(new []{"Invalid access token"});

            var errorMessages = _eventsService.GetEventsLastWeek(authId, out var events);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

            return Ok(events);
        }
    }
}
