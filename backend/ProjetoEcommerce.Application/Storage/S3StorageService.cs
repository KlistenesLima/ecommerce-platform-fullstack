using Amazon.S3;
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
        private readonly AWSSettings _awsSettings; // Injetamos as configs aqui

        public S3StorageService(IAmazonS3 s3Client, AWSSettings awsSettings)
        {
            _s3Client = s3Client;
            _awsSettings = awsSettings;
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string originalFileName, string contentType)
        {
            try
            {
                // 1. Gera nome único para o arquivo
                var extension = Path.GetExtension(originalFileName);
                var fileName = $"{Guid.NewGuid()}{extension}";

                // 2. Prepara o upload
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = fileStream,
                    Key = fileName,
                    BucketName = _awsSettings.S3BucketName,
                    CannedACL = S3CannedACL.PublicRead, // Permite ver a imagem no site
                    ContentType = contentType
                };

                // 3. Faz o upload
                var fileTransferUtility = new TransferUtility(_s3Client);
                await fileTransferUtility.UploadAsync(uploadRequest);

                // 4. Monta a URL correta do Backblaze
                // Pega do appsettings: https://s3.us-west-005.backblazeb2.com
                var serviceUrl = _awsSettings.ServiceUrl;

                if (serviceUrl.EndsWith("/"))
                    serviceUrl = serviceUrl.TrimEnd('/');

                // Retorna: https://s3.us-west-005.backblazeb2.com/luxestore-media/arquivo.jpg
                return $"{serviceUrl}/{_awsSettings.S3BucketName}/{fileName}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO S3]: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteFileAsync(string fileUrl)
        {
            try
            {
                // Tenta extrair apenas o nome do arquivo da URL completa
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
            // Método auxiliar caso precise apenas montar a URL
            var serviceUrl = _awsSettings.ServiceUrl.TrimEnd('/');
            return $"{serviceUrl}/{_awsSettings.S3BucketName}/{key}";
        }
    }
}