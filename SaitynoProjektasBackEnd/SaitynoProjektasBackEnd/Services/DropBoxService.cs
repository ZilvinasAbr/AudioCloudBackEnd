using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;
using Microsoft.AspNetCore.Http;

namespace SaitynoProjektasBackEnd.Services
{
    public class DropBoxService : IDropBoxService
    {
        private readonly string _accessToken;

        public DropBoxService()
        {
            _accessToken = "<INSERT HERE DROPBOX ACCESS TOKEN>";
        }

        public async Task<string[]> DeleteFile(string filePath)
        {
            using (var dbx = new DropboxClient(_accessToken))
            {
                if (string.IsNullOrEmpty(filePath))
                    return new[] {"Song was deleted, but song file does not exist in the file storage"};

                var result = await dbx.Files.DeleteV2Async($"/{filePath}");

                if (!result.Metadata.IsFile)
                    return new[] {"Could not find song file in the file storage"};

                return null;
            }
        }

        public async Task<bool> DoesFileExist(string filePath)
        {
            using (var dbx = new DropboxClient(_accessToken))
            {
                var searchArg = new SearchArg(path: "", query: filePath);

                var searchResult = await dbx.Files.SearchAsync(searchArg);

                var matches = searchResult.Matches.Where(m => m.MatchType.IsFilename);

                return matches.Any();
            }
        }

        public async Task<FileMetadata> UploadFile(IFormFile file)
        {
            using (var dbx = new DropboxClient(_accessToken))
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
