using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SaitynoProjektasBackEnd.Models;
using SaitynoProjektasBackEnd.Services;

namespace SaitynoProjektasBackEnd.Controllers
{
    [Route("api/[controller]")]
    public class LikesController : Controller
    {
        private readonly ILikesService _likesService;
        private readonly IUsersService _usersService;

        public LikesController(ILikesService likesService, IUsersService usersService)
        {
            _likesService = likesService;
            _usersService = usersService;
        }

        [Authorize]
        [HttpPost("Song/{songId}")]
        public IActionResult LikeASong(int songId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));

            var authId = _usersService.GetUserAuthId(User);
            if (authId == null)
                return BadRequest(new[] {"Bad access token provided"});

            var errorMessages = _likesService.LikeASong(songId, authId);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("Song/{songId}")]
        public IActionResult DislikeASong(int songId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));

            var authId = _usersService.GetUserAuthId(User);
            if (authId == null)
                return BadRequest(new[] {"Bad access token provided"});

            var errorMessages = _likesService.DislikeASong(songId, authId);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

            return NoContent();
        }

        [Authorize]
        [HttpPost("Playlist/{playlistId}")]
        public IActionResult LikeAPlaylist(int playlistId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));

            var authId = _usersService.GetUserAuthId(User);
            if (authId == null)
                return BadRequest(new[] {"Bad access token provided"});

            var errorMessages = _likesService.LikeAPlaylist(playlistId, authId);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("Playlist/{playlistId}")]
        public IActionResult DislikeAPlaylist(int playlistId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));

            var authId = _usersService.GetUserAuthId(User);
            if (authId == null)
                return BadRequest(new[] {"Bad access token provided"});

            var errorMessages = _likesService.DislikeAPlaylist(playlistId, authId);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

            return NoContent();
        }
    }
}
