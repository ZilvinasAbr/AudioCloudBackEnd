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

        public async Task<Stream> DownloadFileAsync(string filePath)
        {
            using (var dbx = new DropboxClient(_accessToken))
            {
                var response = await dbx.Files.DownloadAsync($"/{filePath}");

                var stream = await response.GetContentAsStreamAsync();

                return stream;
            }
        }

        public async Task<FileMetadata> UploadFileAsync(IFormFile file)
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
