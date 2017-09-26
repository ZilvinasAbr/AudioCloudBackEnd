using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SaitynoProjektasBackEnd.Services;

namespace SaitynoProjektasBackEnd.Controllers
{
    [Route("api/[controller]")]
    public class FilesController : Controller
    {
        private readonly IDropBoxService _dropBoxService;

        public FilesController(IDropBoxService dropBoxService)
        {
            _dropBoxService = dropBoxService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            var files = Request.Form.Files.ToList();
            var file = files.SingleOrDefault();
            if (file == null)
                return BadRequest("File is not attached");

            var errorMessages = await _dropBoxService.UploadFile(file);

            if (errorMessages != null)
            {
                return BadRequest(errorMessages);
            }

            return NoContent();
        }
    }
}
