using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using SaitynoProjektasBackEnd.Data;
using SaitynoProjektasBackEnd.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SaitynoProjektasBackEnd.Controllers
{
    [Route("api/[controller]")]
    public class SongController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SongController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var songs = _context.Songs.ToList();

            return Ok(songs);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var song = _context.Songs.SingleOrDefault(s => s.Id == id);

            if (song == null)
            {
                return NotFound();
            }

            return Ok(song);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Song song)
        {
            _context.Songs.Add(song);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Song song)
        {
            var songFound = _context.Songs.SingleOrDefault(s => s.Id == id);

            if (songFound == null)
            {
                return NotFound();
            }

            _context.Songs.Update(song);
            _context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var songFound = _context.Songs.SingleOrDefault(s => s.Id == id);

            if (songFound == null)
            {
                return NotFound();
            }

            _context.Songs.Remove(songFound);
            _context.SaveChanges();

            return Ok();
        }
    }
}
