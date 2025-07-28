using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using LaptopRentalManagement.BLL.Interfaces;


namespace LaptopRentalManagement.Services
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
        private readonly long _maxFileSize = 5 * 1024 * 1024; //

        public FileUploadService(IAmazonS3 s3Client)
        {
            _s3Client = s3Client;
            _bucketName = Environment.GetEnvironmentVariable("BUCKET_NAME")
                          ?? throw new ArgumentException("Missing BUCKET_NAME env var");
        }

        public async Task<string?> UploadImageAsync(IFormFile imageFile, string folder = "laptops")
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            if (imageFile.Length > _maxFileSize)
                throw new InvalidOperationException("File size exceeds 5MB.");

            var ext = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(ext))
                throw new InvalidOperationException("Invalid file type.");

            var key = $"{folder}/{Guid.NewGuid()}{ext}";

            using var ms = new MemoryStream();
            await imageFile.CopyToAsync(ms);
            ms.Position = 0;

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = ms,
                BucketName = _bucketName,
                Key = key,
                ContentType = imageFile.ContentType
            };

            var util = new TransferUtility(_s3Client);
            await util.UploadAsync(uploadRequest);

            return $"https://{_bucketName}.s3.amazonaws.com/{key}";
        }

        public async Task<bool> DeleteImageAsync(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                return false;

            try
            {
                var uri = new Uri(fileUrl);
                var key = uri.AbsolutePath.TrimStart('/');

                var deleteReq = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = key
                };
                await _s3Client.DeleteObjectAsync(deleteReq);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}