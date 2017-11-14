using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SaitynoProjektasBackEnd.OtherModels;
using SaitynoProjektasBackEnd.Services.Interfaces;

namespace SaitynoProjektasBackEnd.Services.Classes
{
    public class DropBoxService : IDropBoxService
    {
        private readonly string _accessToken;

        public DropBoxService(IConfiguration configuration)
        {
            _accessToken = configuration.GetConnectionString("DropBox");
        }

        public async Task<bool> DeleteFileAsync(string filePath)
        {
            using (var dbx = new DropboxClient(_accessToken))
            {
                if (string.IsNullOrEmpty(filePath))
                    throw new Exception("Song was deleted, but song file does not exist in the file storage");

                var result = await dbx.Files.DeleteV2Async($"/{filePath}");

                if (!result.Metadata.IsFile)
                    throw new Exception("Could not find song file in the file storage");

                return true;
            }
        }

        public async Task<bool> DoesFileExistAsync(string filePath)
        {
            using (var dbx = new DropboxClient(_accessToken))
            {
                var searchArg = new SearchArg(path: "", query: filePath);

                var searchResult = await dbx.Files.SearchAsync(searchArg);

                var matches = searchResult.Matches.Where(m => m.MatchType.IsFilename);

                return matches.Any();
            }
        }

        public async Task<FileModel> DownloadFileAsync(string filePath)
        {
            using (var dbx = new DropboxClient(_accessToken))
            {
                var path = $"/{filePath}";

                var metaData = await dbx.Files.GetMetadataAsync(path);

                var size = metaData.AsFile.Size;

                var response = await dbx.Files.DownloadAsync(path);

                // var stream = await response.GetContentAsStreamAsync();

                var data = await response.GetContentAsByteArrayAsync();

                var stream = new MemoryStream(data);

                var fileModel = new FileModel
                {
                    Stream = stream,
                    Size = size
                };

                return fileModel;
            }
        }

        public async Task<FileMetadata> UploadFileAsync(IFormFile file)
        {
            using (var dbx = new DropboxClient(_accessToken))
            {
                var fileName = $"/{Guid.NewGuid()}{file.FileName}";

                var readStream = file.OpenReadStream();

                var fileMetaData = await dbx.Files.UploadAsync(
                    fileName,
                    WriteMode.Overwrite.Instance,
                    body: readStream);
                return fileMetaData;
            }
        }
    }
}
