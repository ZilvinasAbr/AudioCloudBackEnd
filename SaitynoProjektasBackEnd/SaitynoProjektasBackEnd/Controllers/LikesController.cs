using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SaitynoProjektasBackEnd.Models;
using SaitynoProjektasBackEnd.Services;

namespace SaitynoProjektasBackEnd.Controllers
{
    [Route("api/[controller]")]
    public class LikesController : Controller
    {
        private readonly ILikesService _likesService;

        public LikesController(ILikesService likesService)
        {
            _likesService = likesService;
        }

        [HttpPost("Song/{songId}")]
        public IActionResult LikeASong(int songId, [FromHeader] string userName)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelStateHandler.GetModelStateErrors(ModelState);

                return BadRequest(modelErrors.ToArray());
            }

            var errorMessages = _likesService.LikeASong(songId, userName);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

            return NoContent();
        }

        [HttpDelete("Song/{songId}")]
        public IActionResult DislikeASong(int songId, [FromHeader] string userName)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelStateHandler.GetModelStateErrors(ModelState);

                return BadRequest(modelErrors.ToArray());
            }

            var errorMessages = _likesService.DislikeASong(songId, userName);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

            return NoContent();
        }


        [HttpPost("Playlist/{playlistId}")]
        public IActionResult LikeAPlaylist(int playlistId, [FromHeader]string userName)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelStateHandler.GetModelStateErrors(ModelState);

                return BadRequest(modelErrors.ToArray());
            }

            var errorMessages = _likesService.LikeAPlaylist(playlistId, userName);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

            return NoContent();
        }

        [HttpDelete("Playlist/{playlistId}")]
        public IActionResult DislikeAPlaylist(int playlistId, [FromHeader] string userName)
        {
            if (!ModelState.IsValid)
            {
                var modelErrors = ModelStateHandler.GetModelStateErrors(ModelState);

                return BadRequest(modelErrors.ToArray());
            }

            var errorMessages = _likesService.DislikeAPlaylist(playlistId, userName);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

            return NoContent();
        }
    }
}
