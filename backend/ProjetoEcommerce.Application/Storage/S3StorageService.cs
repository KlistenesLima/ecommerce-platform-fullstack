using Microsoft.Extensions.Options;
using ProjetoEcommerce.Application.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Application.Storage
{
    public class S3StorageService : IS3StorageService
    {
        private readonly AWSSettings _awsSettings;

        public S3StorageService(IOptions<AWSSettings> awsSettings)
        {
            _awsSettings = awsSettings.Value;
        }

        public Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            // Simulação (Retorna uma URL fictícia)
            // O contentType não está sendo usado na simulação, mas é necessário para a assinatura
            var fakeUrl = $"https://{_awsSettings.S3BucketName}.s3.amazonaws.com/{Guid.NewGuid()}_{fileName}";
            return Task.FromResult(fakeUrl);
        }

        public Task<bool> DeleteFileAsync(string fileName)
        {
            // Simulação (Sempre retorna sucesso)
            return Task.FromResult(true);
        }

        public string GetFileUrl(string fileName)
        {
            return $"https://{_awsSettings.S3BucketName}.s3.amazonaws.com/{fileName}";
        }
    }
}
