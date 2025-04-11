using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.Identity.Client;

namespace EduConnect.Services
{
    public class AzureBlobStorageService
    {
        private readonly string _connectionString;
        private readonly string _containerName;

        private readonly BlobServiceClient _blobServiceClient;
        public AzureBlobStorageService(IConfiguration configuration)
        {
            this._connectionString = configuration["AzureBlobStorage:ConnectionString"];
            this._containerName = configuration["AzureBlobStorage:BlobContainerName"];

            Console.WriteLine($"Connection to Azure Blob Storage for Course Thumbnails, with connection string <<{this._connectionString}>> and container name <<{this._containerName}>>");
            this._blobServiceClient = new BlobServiceClient(this._connectionString);

        }

        public async Task<string> UploadCourseThumbnailAsync(IFormFile file, string blobName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync();


            var blobClient = containerClient.GetBlobClient(blobName);

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            return blobClient.Uri.ToString();

        }

        public async Task<(Stream Stream, string ContentType)> DownloadCourseThumbnailAsync(Guid courseId)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(courseId.ToString());

            if (!await blobClient.ExistsAsync())
            {
                throw new FileNotFoundException("Course Thumbnail does not exist");
            }

            var response = await blobClient.DownloadAsync();
            return (response.Value.Content, response.Value.ContentType);
        }

        public async Task<bool> DeleteCourseThumbnailAsync(Guid courseId)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                var blobClient = containerClient.GetBlobClient(courseId.ToString());

                if (!await blobClient.ExistsAsync())
                {
                    return false;
                }

                await blobClient.DeleteAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting blob {courseId}: {ex.Message}");
                return false;
            }
        }


    }
}