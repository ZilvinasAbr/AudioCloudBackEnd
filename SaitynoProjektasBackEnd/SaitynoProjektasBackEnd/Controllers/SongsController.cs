using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SaitynoProjektasBackEnd.Data;
using SaitynoProjektasBackEnd.Models;

namespace SaitynoProjektasBackEnd.Controllers
{
    [Route("api/[controller]")]
    public class SongsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SongsController(ApplicationDbContext context)
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
