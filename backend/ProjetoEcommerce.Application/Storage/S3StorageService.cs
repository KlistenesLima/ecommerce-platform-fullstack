using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using ProjetoEcommerce.Application.Configuration;
using ProjetoEcommerce.Application.Storage;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ProjetoEcommerce.Infra.Storage
{
    public class S3StorageService : IS3StorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly AWSSettings _awsSettings;

        // URL específica para DOWNLOAD público do Backblaze (Região 005)
        private const string BackblazePublicUrl = "https://f005.backblazeb2.com/file";

        public S3StorageService(IAmazonS3 s3Client, AWSSettings awsSettings)
        {
            _s3Client = s3Client;
            _awsSettings = awsSettings;
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string originalFileName, string contentType)
        {
            try
            {
                var extension = Path.GetExtension(originalFileName);
                var fileName = $"{Guid.NewGuid()}{extension}";

                // Copia para memória para evitar erros de stream
                using var memoryStream = new MemoryStream();
                await fileStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = memoryStream,
                    Key = fileName,
                    BucketName = _awsSettings.S3BucketName,
                    CannedACL = S3CannedACL.PublicRead,
                    ContentType = contentType,
                    AutoCloseStream = false
                };

                // Faz o upload usando o cliente S3 (que aponta para s3.us-west-005...)
                var fileTransferUtility = new TransferUtility(_s3Client);
                await fileTransferUtility.UploadAsync(uploadRequest);

                // === AQUI ESTÁ A CORREÇÃO DA URL ===
                // Retorna a URL "Friendly" do Backblaze que funciona no navegador
                // Formato: https://f005.backblazeb2.com/file/{Bucket}/{Arquivo}
                return $"{BackblazePublicUrl}/{_awsSettings.S3BucketName}/{fileName}";
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> DeleteFileAsync(string fileUrl)
        {
            try
            {
                // Extrai o nome do arquivo da URL (funciona tanto para s3... quanto f005...)
                var uri = new Uri(fileUrl);
                var fileName = Path.GetFileName(uri.LocalPath);

                await _s3Client.DeleteObjectAsync(_awsSettings.S3BucketName, fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetFileUrl(string key)
        {
            // Garante que se precisar recuperar a URL pelo nome, use o formato correto
            return $"{BackblazePublicUrl}/{_awsSettings.S3BucketName}/{key}";
        }
    }
}