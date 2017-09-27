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
    public class PlaylistsController : Controller
    {
        private readonly IPlaylistsService _playlistsService;

        public PlaylistsController(IPlaylistsService playlistsService)
        {
            _playlistsService = playlistsService;
        }

        [HttpGet]
        public IActionResult Get([FromHeader]string userName)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelStateHandler.GetModelStateErrors(ModelState);

                return BadRequest(modelErrors.ToArray());
            }

            var errorMessages = _playlistsService.GetPlaylists(userName, out IEnumerable<PlaylistResponseModel> playlists);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

            return Ok(playlists);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id, [FromHeader] string userName)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelStateHandler.GetModelStateErrors(ModelState);

                return BadRequest(modelErrors.ToArray());
            }

            var errorMessages = _playlistsService.GetPlaylistById(id, userName, out PlaylistResponseModel playlist);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

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

        [HttpPost("{playlistId}/Song/{songId}")]
        public IActionResult AddSong(int playlistId, int songId, [FromHeader]string userName)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelStateHandler.GetModelStateErrors(ModelState);

                return BadRequest(modelErrors.ToArray());
            }

            var errorMessages = _playlistsService.AddSong(playlistId, songId, userName);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

            return NoContent();
        }

        [HttpDelete("{playlistId}/Song/{songId}")]
        public IActionResult RemoveSong(int playlistId, int songId, [FromHeader] string userName)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelStateHandler.GetModelStateErrors(ModelState);

                return BadRequest(modelErrors.ToArray());
            }

            var errorMessages = _playlistsService.RemoveSong(playlistId, songId, userName);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

            return NoContent();
        }

        [HttpGet("User/{userNameOfPlaylists}")]
        public IActionResult GetUserPlaylists(string userNameOfPlaylists, [FromHeader] string userName)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelStateHandler.GetModelStateErrors(ModelState);

                return BadRequest(modelErrors.ToArray());
            }

            IEnumerable<PlaylistResponseModel> playlists = null;
            
            var errorMessages = _playlistsService.GetUserPlaylists(userNameOfPlaylists, userName, out playlists);

            return Ok(playlists);
        }
    }
}
