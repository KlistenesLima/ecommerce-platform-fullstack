using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;

namespace ProjetoEcommerce.Application.Storage
{
    public class S3StorageService : IS3StorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly string _region;

        public S3StorageService(IAmazonS3 s3Client, IConfiguration configuration)
        {
            _s3Client = s3Client;
            _bucketName = configuration["AWS:S3BucketName"] ?? "ecommerce-bucket";
            _region = configuration["AWS:Region"] ?? "us-east-1";
        }

        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
        {
            var extension = Path.GetExtension(fileName);
            var key = $"images/{Guid.NewGuid()}{extension}";

            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = fileStream,
                ContentType = contentType,
                CannedACL = S3CannedACL.PublicRead
            };

            await _s3Client.PutObjectAsync(request);

            return GetFileUrl(key);
        }

        public async Task<bool> DeleteFileAsync(string fileUrl)
        {
            try
            {
                var key = ExtractKeyFromUrl(fileUrl);
                if (string.IsNullOrEmpty(key)) return false;

                var request = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = key
                };

                await _s3Client.DeleteObjectAsync(request);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetFileUrl(string key)
        {
            return $"https://{_bucketName}.s3.{_region}.amazonaws.com/{key}";
        }

        private string ExtractKeyFromUrl(string url)
        {
            var prefix = $"https://{_bucketName}.s3.{_region}.amazonaws.com/";
            if (url.StartsWith(prefix))
            {
                return url.Substring(prefix.Length);
            }
            return string.Empty;
        }
    }
}
