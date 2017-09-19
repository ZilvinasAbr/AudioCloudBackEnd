﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SaitynoProjektasBackEnd.Data;
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
            var result = _songsService.AddSong(song);

            if (!result)
            {
                return BadRequest();
            }

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Song song)
        {
            return Ok("Not Implemented");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok("Not Implemented");
        }
    }
}
