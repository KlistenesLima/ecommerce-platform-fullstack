using System.IO;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Application.Storage
{
    public interface IS3StorageService
    {
        // Contrato alinhado com o que vamos implementar
        Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType);
        Task<bool> DeleteFileAsync(string fileName);
        string GetFileUrl(string fileName);
    }
}
