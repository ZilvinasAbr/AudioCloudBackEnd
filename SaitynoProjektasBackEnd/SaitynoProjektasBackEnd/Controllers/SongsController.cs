using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SaitynoProjektasBackEnd.Models;
using SaitynoProjektasBackEnd.RequestModels;
using SaitynoProjektasBackEnd.Services;

namespace SaitynoProjektasBackEnd.Controllers
{
    [Route("api/[controller]")]
    public class SongsController : Controller
    {
        private readonly ISongsService _songsService;

        public SongsController(ISongsService songsService)
        {
            _songsService = songsService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var songs = _songsService.GetSongs();

            return Ok(songs);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var song = _songsService.GetSongById(id);
            
            if (song == null)
            {
                return NotFound();
            }

            return Ok(song);
        }

        [HttpPost]
        public IActionResult Post([FromBody]AddSongRequestModel song)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelStateHandler.GetModelStateErrors(ModelState);

                return BadRequest(modelErrors.ToArray());
            }

            var errorMessages = _songsService.AddSong(song);

            if (errorMessages != null)
            {
                return Forbid(errorMessages);
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]EditSongRequestModel song)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelStateHandler.GetModelStateErrors(ModelState);

                return BadRequest(modelErrors.ToArray());
            }

            var errorMessages = _songsService.EditSong(id, song);

            if (errorMessages != null)
            {
                return Forbid(errorMessages);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var errorMessages = _songsService.DeleteSong(id);

            if (errorMessages != null)
            {
                return Forbid(errorMessages);
            }

            return NoContent();
        }
    }
}
