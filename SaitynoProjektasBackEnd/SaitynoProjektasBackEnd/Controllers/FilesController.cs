using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SaitynoProjektasBackEnd.Models;
using SaitynoProjektasBackEnd.Services;
using SaitynoProjektasBackEnd.Services.Interfaces;

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
        return BadRequest(new[] { "Bad access token provided" });

      if (toUpload == null)
        return BadRequest(new[] { "File is not attached" });

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
      try
      {
        var fileModel = await _dropBoxService.DownloadFileAsync(filePath);

        var stream = fileModel.Stream;
        var size = fileModel.Size;

        var fileResponse = File(stream, "audio/mpeg");

        var response = Request.HttpContext.Response;

        response.Headers.ContentLength = (long) size;

        return fileResponse;
      }
      catch (Exception e)
      {
        return BadRequest(new[] { e.Message });
      }
    }
  }
}
