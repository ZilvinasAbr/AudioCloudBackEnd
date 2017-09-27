using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SaitynoProjektasBackEnd.Models;
using SaitynoProjektasBackEnd.Services;

namespace SaitynoProjektasBackEnd.Controllers
{
    [Route("api/[controller]")]
    public class FilesController : Controller
    {
        private readonly IDropBoxService _dropBoxService;
        private readonly ISongsService _songsService;

        public FilesController(IDropBoxService dropBoxService)
        {
            _dropBoxService = dropBoxService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile toUpload)
        {
            if (toUpload == null)
                return BadRequest("File is not attached");

            var fileMetadata = await _dropBoxService.UploadFile(toUpload);

            if (fileMetadata == null)
            {
                return BadRequest();
            }

            var fileName = fileMetadata.Name;

            return Ok(fileName);
        }

        // TODO: Remove completely if not needed.
        // [HttpGet("{query}")]
        // public async Task<IActionResult> DoesFileExist(string query)
        // {
        //     if (!ModelState.IsValid)
        //     {
        //         var modelErrors = ModelStateHandler.GetModelStateErrors(ModelState);

        //         return BadRequest(modelErrors.ToArray());
        //     }

        //     var result = await _dropBoxService.DoesFileExist(query);
        //     return Ok(result);
        // }
    }
}
