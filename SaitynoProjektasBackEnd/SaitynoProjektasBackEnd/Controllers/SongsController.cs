using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUsersService _usersService;

        public SongsController(ISongsService songsService, IUsersService usersService)
        {
            _songsService = songsService;
            _usersService = usersService;
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
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));

            try
            {
                var songs = _songsService.GetSongsByGenre(genreName);

                return Ok(songs);
            }
            catch (Exception e)
            {
                return BadRequest(new[] {e.Message});
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));

            var song = _songsService.GetSongById(id);
            
            if (song == null)
            {
                return BadRequest();
            }

            return Ok(song);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddSongRequestModel song)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));

            var authId = _usersService.GetUserAuthId(User);
            if (authId == null)
                return BadRequest(new[] {"Bad access token provided"});

            try
            {
                var songCreated = await _songsService.AddSong(song, authId);
                return Created($"api/songs/{songCreated.Id}", songCreated.Id);
            }
            catch (Exception e)
            {
                return BadRequest(new[]{e.Message});
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]EditSongRequestModel song)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));

            var authId = _usersService.GetUserAuthId(User);
            if (authId == null)
                return BadRequest(new[] {"Bad access token provided"});

            try
            {
                _songsService.EditSong(id, song, authId);

                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(new[] { e.Message });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));
            
            var authId = _usersService.GetUserAuthId(User);
            if (authId == null)
                return BadRequest(new[] {"Bad access token provided"});

            try
            {
                var success = await _songsService.DeleteSong(id, authId);

                if (success)
                    return Ok();
                return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(new[] { e.Message });
            }
        }

        [HttpGet("Search/{query}")]
        public IActionResult Search(string query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));

            var songs = _songsService.SearchSongs(query);

            return Ok(songs);
        }

        [Authorize]
        [HttpGet("user/{userName}")]
        public IActionResult GetUserSongs(string userName)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));
            
            try
            {
                var userSongs = _songsService.GetUserSongs(userName);

                return Ok(userSongs);
            }
            catch (Exception e)
            {
                return BadRequest(new[] {e.Message});
            }
        }
    }
}
