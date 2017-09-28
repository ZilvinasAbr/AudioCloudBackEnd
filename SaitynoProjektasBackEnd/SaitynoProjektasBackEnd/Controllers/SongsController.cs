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

            var errorMessages = await _songsService.AddSong(song, authId);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

            return NoContent();
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

            var errorMessages = _songsService.EditSong(id, song, authId);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

            return NoContent();
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

            var errorMessages = await _songsService.DeleteSong(id, authId);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

            return NoContent();
        }

        [HttpPost("Search")]
        public IActionResult Search([FromBody] SongSearchRequestModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));

            var songs = _songsService.SearchSongs(model.Query);

            return Ok(songs);
        }

        [Authorize]
        [HttpGet("user/{userName}")]
        public IActionResult GetUserSongs(string userName)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));

            var errorMessages = _songsService.GetUserSongs(userName, out var userSongs);

            if (errorMessages != null)
                return BadRequest(errorMessages);

            return Ok(userSongs);
        }
    }
}
