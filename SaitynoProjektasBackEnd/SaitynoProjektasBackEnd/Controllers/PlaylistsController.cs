using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SaitynoProjektasBackEnd.Data;
using SaitynoProjektasBackEnd.Models;
using SaitynoProjektasBackEnd.RequestModels;
using SaitynoProjektasBackEnd.Services;

namespace SaitynoProjektasBackEnd.Controllers
{
    [Route("api/[controller]")]
    public class PlaylistsController : Controller
    {
        private readonly IPlaylistsService _playlistsService;

        public PlaylistsController(IPlaylistsService playlistsService)
        {
            _playlistsService = playlistsService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var playlists = _playlistsService.GetPlaylists();

            return Ok(playlists);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var playlist = _playlistsService.GetPlaylistById(id);

            if (playlist == null)
            {
                return NotFound();
            }

            return Ok(playlist);
        }

        [HttpPost]
        public IActionResult Post([FromBody]AddPlaylistRequestModel playlist)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelStateHandler.GetModelStateErrors(ModelState);

                return BadRequest(modelErrors.ToArray());
            }

            var errorMessages = _playlistsService.AddPlaylist(playlist);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]EditPlaylistRequestModel playlist)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelStateHandler.GetModelStateErrors(ModelState);

                return BadRequest(modelErrors.ToArray());
            }

            var errorMessages = _playlistsService.EditPlaylist(id, playlist);

            if (errorMessages != null)
            {
                return Forbid(errorMessages);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var errorMessages = _playlistsService.DeletePlaylist(id);

            if (errorMessages != null)
            {
                return Forbid(errorMessages);
            }

            return NoContent();
        }
    }
}
