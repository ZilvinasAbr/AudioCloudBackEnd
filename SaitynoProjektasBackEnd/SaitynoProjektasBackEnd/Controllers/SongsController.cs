using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SaitynoProjektasBackEnd.Models;
using SaitynoProjektasBackEnd.RequestModels;
using SaitynoProjektasBackEnd.ResponseModels;
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

        [HttpGet("Genre")]
        public IActionResult GetByGenre([FromQuery] string genreName)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelStateHandler.GetModelStateErrors(ModelState);

                return BadRequest(modelErrors.ToArray());
            }

            var errorMessages = _songsService.GetSongsByGenre(genreName, out IEnumerable<SongResponseModel> songs);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

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
        public async Task<IActionResult> Post([FromBody]AddSongRequestModel song, [FromHeader]string userName)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelStateHandler.GetModelStateErrors(ModelState);

                return BadRequest(modelErrors.ToArray());
            }

            var errorMessages = await _songsService.AddSong(song, userName);

            if (errorMessages != null)
            {
                return Forbid(errorMessages);
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]EditSongRequestModel song, [FromHeader] string userName)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelStateHandler.GetModelStateErrors(ModelState);

                return BadRequest(modelErrors.ToArray());
            }

            var errorMessages = _songsService.EditSong(id, song, userName);

            if (errorMessages != null)
            {
                return Forbid(errorMessages);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromHeader] string userName)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelStateHandler.GetModelStateErrors(ModelState);

                return BadRequest(modelErrors.ToArray());
            }
            
            var errorMessages = await _songsService.DeleteSong(id, userName);

            if (errorMessages != null)
            {
                return Forbid(errorMessages);
            }

            return NoContent();
        }

        [HttpPost("Search")]
        public IActionResult Search([FromBody] SongSearchRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelStateHandler.GetModelStateErrors(ModelState);

                return BadRequest(modelErrors.ToArray());
            }

            var songs = _songsService.SearchSongs(model.Query);

            return Ok(songs);
        }
    }
}
