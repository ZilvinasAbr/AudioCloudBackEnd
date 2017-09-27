using System.IO;
using System.Threading.Tasks;
using Dropbox.Api.Files;
using Microsoft.AspNetCore.Http;

namespace SaitynoProjektasBackEnd.Services
{
    public interface IDropBoxService
    {
        Task<FileMetadata> UploadFileAsync(IFormFile file);
        Task<bool> DoesFileExistAsync(string filePath);
        Task<string[]> DeleteFileAsync(string filePath);
        Task<Stream> DownloadFileAsync(string filePath);
    }
}
