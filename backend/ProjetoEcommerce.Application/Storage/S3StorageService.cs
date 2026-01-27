using Amazon.S3;
using Amazon.S3.Model;
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

                Console.WriteLine($"=== DEBUG S3 UPLOAD ===");
                Console.WriteLine($"FileName: {fileName}");
                Console.WriteLine($"BucketName: {_awsSettings.S3BucketName}");

                using var memoryStream = new MemoryStream();
                await fileStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                var putRequest = new PutObjectRequest
                {
                    BucketName = _awsSettings.S3BucketName,
                    Key = fileName,
                    InputStream = memoryStream,
                    ContentType = contentType,
                    CannedACL = S3CannedACL.PublicRead
                };

                Console.WriteLine($"Iniciando upload com PutObjectAsync...");
                var response = await _s3Client.PutObjectAsync(putRequest);
                Console.WriteLine($"Upload concluído! Status: {response.HttpStatusCode}");

                return $"https://{_awsSettings.S3BucketName}.s3.us-west-005.backblazeb2.com/{fileName}";
            }
            catch (AmazonS3Exception s3Ex)
            {
                Console.WriteLine($"=== ERRO S3 ===");
                Console.WriteLine($"ErrorCode: {s3Ex.ErrorCode}");
                Console.WriteLine($"StatusCode: {s3Ex.StatusCode}");
                Console.WriteLine($"Message: {s3Ex.Message}");
                throw new Exception($"Erro S3: {s3Ex.Message} - ErrorCode: {s3Ex.ErrorCode}", s3Ex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"=== ERRO GERAL ===");
                Console.WriteLine($"Message: {ex.Message}");
                Console.WriteLine($"InnerException: {ex.InnerException?.Message}");
                throw new Exception($"Erro ao fazer upload: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteFileAsync(string fileUrl)
        {
            try
            {
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
            return $"https://{_awsSettings.S3BucketName}.s3.us-west-005.backblazeb2.com/{key}";
        }
    }
}