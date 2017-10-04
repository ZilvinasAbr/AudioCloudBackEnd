using System;
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

            try
            {
                var playlists = _playlistsService.GetPlaylists(authId);
                return Ok(playlists);
            }
            catch (Exception e)
            {
                return BadRequest(new[] {e.Message});
            }
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

            try
            {
                var playlist = _playlistsService.GetPlaylistById(id, authId);

                if (playlist == null)
                    return NotFound();

                return Ok(playlist);
            }
            catch (Exception e)
            {
                return BadRequest(new[] {e.Message});
            }
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

            try
            {
                var playlistCreated = _playlistsService.AddPlaylist(playlist, authId);

                return Created($"api/playlists/{playlistCreated.Id}", playlistCreated.Id);
            }
            catch (Exception e)
            {
                return BadRequest(new[] { e.Message });
            }
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

            try
            {
                _playlistsService.EditPlaylist(id, playlist, authId);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(new[] { e.Message });
            }
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

            try
            {
                _playlistsService.DeletePlaylist(id, authId);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new[] { e.Message });
            }
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

            try
            {
                var playlistSongAdded = _playlistsService.AddSong(playlistId, songId, authId);

                return Created($"{playlistId}/Song/{songId}", new { playlistSongAdded.PlaylistId, playlistSongAdded.SongId });
            }
            catch (Exception e)
            {
                return BadRequest(new[] { e.Message });
            }
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

            try
            {
                _playlistsService.RemoveSong(playlistId, songId, authId);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(new[] { e.Message });
            }
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

            try
            {
                var playlists = _playlistsService.GetUserPlaylists(userNameOfPlaylists, authId);

                return Ok(playlists);
            }
            catch (Exception e)
            {
                return BadRequest(new[] { e.Message });
            }
        }

        [Authorize]
        [HttpGet("Liked")]
        public IActionResult GetUserLikedPlaylist()
        {
            var authId = _usersService.GetUserAuthId(User);
            if (authId == null)
                return BadRequest(new[] {"Bad access token provided"});

            try
            {
                var playlist = _playlistsService.GetUserLikedPlaylist(authId);

                return Ok(playlist);
            }
            catch (Exception e)
            {
                return BadRequest(new[] { e.Message });
            }
        }
    }
}
