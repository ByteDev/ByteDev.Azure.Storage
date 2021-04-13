using System.Collections.Generic;
using System.Threading.Tasks;
using ByteDev.Azure.Storage.File;
using NUnit.Framework;

namespace ByteDev.Azure.Storage.IntTests.File
{
    public abstract class FileTestBase : IntTestBase
    {
        private readonly IList<string> _trackedCloudFiles = new List<string>();

        private readonly FileStorageClient _client;

        protected string ConnectionString;

        protected FileTestBase(string shareName)
        {
            ConnectionString = GetTestConnectionString();

            _client = new FileStorageClient(ConnectionString, shareName, true);
        }

        protected async Task<string> CreateCloudFileAsync(string cloudFilePath)
        {
            const string content = "some test text";

            await _client.UploadTextAsync(content, cloudFilePath);
            TrackCloudFile(cloudFilePath);

            return content;
        }

        protected async Task AssertCloudFileExistsAsync(string cloudFilePath)
        {
            var exists = await _client.ExistsAsync(cloudFilePath);

            Assert.That(exists, Is.True);

            TrackCloudFile(cloudFilePath);
        }

        protected async Task AssertCloudFileTextEqualsAsync(string cloudFilePath, string expected)
        {
            var dlContent = await _client.DownloadTextAsync(cloudFilePath);

            Assert.That(dlContent, Is.EqualTo(expected));

            TrackCloudFile(cloudFilePath);
        }

        protected void DeleteLocalFileIfExists(string filePath)
        {
            if (System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);
        }

        protected async Task DeleteIfExistsAsync(string cloudFilePath)
        {
            await _client.DeleteIfExistsAsync(cloudFilePath);
        }

        protected void TrackCloudFile(string cloudFilePath)
        {
            if (!_trackedCloudFiles.Contains(cloudFilePath))
                _trackedCloudFiles.Add(cloudFilePath);
        }

        protected async Task CleanUpFilesAsync()
        {
            await _client.DeleteIfExistsAsync(_trackedCloudFiles);

            _trackedCloudFiles.Clear();
        }
    }
}