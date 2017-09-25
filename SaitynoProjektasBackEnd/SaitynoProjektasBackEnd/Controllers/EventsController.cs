using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SaitynoProjektasBackEnd.Models;
using SaitynoProjektasBackEnd.RequestModels;
using SaitynoProjektasBackEnd.ResponseModels;
using SaitynoProjektasBackEnd.Services;

namespace SaitynoProjektasBackEnd.Controllers
{
    [Route("api/[controller]")]
    public class EventsController : Controller
    {
        private readonly IEventsService _eventsService;

        public EventsController(IEventsService eventsService)
        {
            _eventsService = eventsService;
        }

        [HttpGet]
        public IActionResult Get([FromHeader]string userName)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelStateHandler.GetModelStateErrors(ModelState);

                return BadRequest(modelErrors.ToArray());
            }

            var errorMessages = _eventsService.GetEvents(userName, out var events);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

            return Ok(events);
        }

        [HttpGet("lastWeek")]
        public IActionResult GetLastWeek([FromHeader] string userName)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelStateHandler.GetModelStateErrors(ModelState);

                return BadRequest(modelErrors.ToArray());
            }

            var errorMessages = _eventsService.GetEventsLastWeek(userName, out var events);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

            return Ok(events);
        }
    }
}
