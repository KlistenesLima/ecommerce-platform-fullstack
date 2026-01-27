namespace ProjetoEcommerce.Application.Configuration // <--- Namespace novo
{
    public class AWSSettings
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string ServiceUrl { get; set; }
        public string S3BucketName { get; set; }
    }
}