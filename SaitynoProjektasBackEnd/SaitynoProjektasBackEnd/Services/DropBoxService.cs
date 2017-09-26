using System;
using System.IO;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.AspNetCore.Http;

namespace SaitynoProjektasBackEnd.Services
{
    public class DropBoxService : IDropBoxService
    {
        public async Task<FileMetadata> UploadFile(IFormFile file)
        {
            using (var dbx = new DropboxClient("<DROP BOX ACCESS TOKEN>"))
            {
                var fileName = "/" + Guid.NewGuid() + file.FileName;
                var data = new MemoryStream();
                file.CopyTo(data);

                var fileMetaData = await dbx.Files.UploadAsync(
                    fileName,
                    WriteMode.Overwrite.Instance,
                    body: data);
                return fileMetaData;
            }
        }
    }
}
