using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using ByteDev.Azure.Storage.Blob;

namespace ByteDev.Azure.Storage.IntTests.Blob
{
    public abstract class BlobTestBase : IntTestBase
    {
        private readonly ConcurrentBag<string> _trackedBlobs = new ConcurrentBag<string>();

        private readonly BlobStorageClient _client;

        protected string ConnectionString;

        protected BlobTestBase(string containerName)
        {
            ConnectionString = GetTestConnectionString();

            _client = new BlobStorageClient(ConnectionString, containerName, true);
        }

        protected void TrackBlob(string blobName)
        {
            _trackedBlobs.Add(blobName);
        }

        protected async Task<string> SaveBlobAsync(string blobNamePrefix, string data)
        {
            string blobName = BlobNameFactory.Create(blobNamePrefix);

            await _client.SaveAsync(blobName, data);

            TrackBlob(blobName);

            return blobName;
        }

        protected void CleanUpBlobs()
        {
            var tasks = new List<Task>();

            foreach (var blobName in _trackedBlobs)
            {
                tasks.Add(_client.DeleteAsync(blobName));
            }

            Task.WaitAll(tasks.ToArray());

            _trackedBlobs.Clear();
        }
    }
}