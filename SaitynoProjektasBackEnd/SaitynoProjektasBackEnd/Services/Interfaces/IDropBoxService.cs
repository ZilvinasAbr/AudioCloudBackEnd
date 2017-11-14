using System.IO;
using System.Threading.Tasks;
using Dropbox.Api.Files;
using Microsoft.AspNetCore.Http;
using SaitynoProjektasBackEnd.OtherModels;

namespace SaitynoProjektasBackEnd.Services.Interfaces
{
    public interface IDropBoxService
    {
        Task<FileMetadata> UploadFileAsync(IFormFile file);
        Task<bool> DoesFileExistAsync(string filePath);
        Task<bool> DeleteFileAsync(string filePath);
        Task<FileModel> DownloadFileAsync(string filePath);
    }
}
