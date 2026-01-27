using System.IO;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Application.Storage
{
    public interface IS3StorageService
    {
        Task<string> UploadFileAsync(Stream fileStream, string originalFileName, string contentType);
        Task<bool> DeleteFileAsync(string fileUrl);
        string GetFileUrl(string key);
    }
}