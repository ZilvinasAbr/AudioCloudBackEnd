using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUsersService _usersService;

        public PlaylistsController(IPlaylistsService playlistsService, IUsersService usersService)
        {
            _playlistsService = playlistsService;
            _usersService = usersService;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));

            var authId = _usersService.GetUserAuthId(User);
            if (authId == null)
                return BadRequest(new[] {"Bad access token provided"});

            var errorMessages = _playlistsService.GetPlaylists(authId, out var playlists);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

            return Ok(playlists);
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));

            var authId = _usersService.GetUserAuthId(User);
            if (authId == null)
                return BadRequest(new[] {"Bad access token provided"});

            var errorMessages = _playlistsService.GetPlaylistById(id, authId, out PlaylistResponseModel playlist);

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

        [Authorize]
        [HttpPost]
        public IActionResult Post([FromBody]AddPlaylistRequestModel playlist)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));

            var authId = _usersService.GetUserAuthId(User);
            if (authId == null)
                return BadRequest(new[] {"Bad access token provided"});

            var errorMessages = _playlistsService.AddPlaylist(playlist, authId, out var playlistCreated);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

            return Created($"api/playlists/{playlistCreated.Id}", playlistCreated.Id);
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]EditPlaylistRequestModel playlist)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));

            var authId = _usersService.GetUserAuthId(User);
            if (authId == null)
                return BadRequest(new[] {"Bad access token provided"});

            var errorMessages = _playlistsService.EditPlaylist(id, playlist, authId);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));

            var authId = _usersService.GetUserAuthId(User);
            if (authId == null)
                return BadRequest(new[] {"Bad access token provided"});
            
            var errorMessages = _playlistsService.DeletePlaylist(id, authId);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

            return Ok();
        }

        [Authorize]
        [HttpPost("{playlistId}/Song/{songId}")]
        public IActionResult AddSong(int playlistId, int songId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));

            var authId = _usersService.GetUserAuthId(User);
            if (authId == null)
                return BadRequest(new[] {"Bad access token provided"});

            var errorMessages = _playlistsService.AddSong(playlistId, songId, authId, out var playlistSongAdded);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

            return Created($"{playlistId}/Song/{songId}", new { playlistSongAdded.PlaylistId, playlistSongAdded.SongId });
        }

        [Authorize]
        [HttpDelete("{playlistId}/Song/{songId}")]
        public IActionResult RemoveSong(int playlistId, int songId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));

            var authId = _usersService.GetUserAuthId(User);
            if (authId == null)
                return BadRequest(new[] {"Bad access token provided"});

            var errorMessages = _playlistsService.RemoveSong(playlistId, songId, authId);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

            return Ok();
        }

        [Authorize]
        [HttpGet("User/{userNameOfPlaylists}")]
        public IActionResult GetUserPlaylists(string userNameOfPlaylists)
        {
           if (!ModelState.IsValid)
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));

            var authId = _usersService.GetUserAuthId(User);
            if (authId == null)
                return BadRequest(new[] {"Bad access token provided"});
            
            var errorMessages = _playlistsService.GetUserPlaylists(userNameOfPlaylists, authId, out var playlists);

            return Ok(playlists);
        }

        [Authorize]
        [HttpGet("Liked")]
        public IActionResult GetUserLikedPlaylist()
        {
            var authId = _usersService.GetUserAuthId(User);
            if (authId == null)
                return BadRequest(new[] {"Bad access token provided"});

            var errorMessages = _playlistsService.GetUserLikedPlaylist(authId, out var playlist);

            if (errorMessages != null)
                return BadRequest(errorMessages);

            return Ok(playlist);
        }
    }
}
