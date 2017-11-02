using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IUsersService _usersService;

        public FilesController(IDropBoxService dropBoxService, IUsersService usersService)
        {
            _dropBoxService = dropBoxService;
            _usersService = usersService;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Upload(IFormFile toUpload)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelStateHandler.GetModelStateErrors(ModelState));

            var authId = _usersService.GetUserAuthId(User);
            if (authId == null)
                return BadRequest(new[] {"Bad access token provided"});

            if (toUpload == null)
                return BadRequest(new[] {"File is not attached"});

            var fileMetadata = await _dropBoxService.UploadFileAsync(toUpload);

            if (fileMetadata == null)
            {
                return BadRequest();
            }

            var fileName = fileMetadata.Name;

            return Created($"api/files/{fileName}", fileName);
        }

        [HttpGet("{filePath}")]
        public async Task<IActionResult> Download(string filePath)
        {
            var stream = await _dropBoxService.DownloadFileAsync(filePath);
            
            var response = File(stream, "application/octet-stream");

            return response;
        }
    }
}
