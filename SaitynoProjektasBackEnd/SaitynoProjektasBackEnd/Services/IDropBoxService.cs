using System.Threading.Tasks;
using Dropbox.Api.Files;
using Microsoft.AspNetCore.Http;

namespace SaitynoProjektasBackEnd.Services
{
    public interface IDropBoxService
    {
        Task<FileMetadata> UploadFile(IFormFile file);
        Task<bool> DoesFileExist(string filePath);
        Task<string[]> DeleteFile(string filePath);
    }
}
