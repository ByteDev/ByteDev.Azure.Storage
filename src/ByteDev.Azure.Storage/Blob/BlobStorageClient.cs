using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace ByteDev.Azure.Storage.Blob
{
    /// <summary>
    /// Represents a client to use Azure Blob Storage.
    /// </summary>
    public class BlobStorageClient : IBlobStorageClient
    {
        private readonly CloudBlobContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Azure.Storage.Blob.BlobStorageClient" /> class.
        /// </summary>
        /// <param name="connectionString">Storage account connection string.</param>
        /// <param name="containerName">Blob container name.</param>
        /// <param name="createContainerIfNotExist">True create the container if it does not exist; otherwise do not create.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="connectionString" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="containerName" /> is null.</exception>
        public BlobStorageClient(string connectionString, string containerName, bool createContainerIfNotExist)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            CloudBlobClient client = storageAccount.CreateCloudBlobClient();

            _container = client.GetContainerReference(containerName);

            if (createContainerIfNotExist)
                _container.CreateIfNotExists();
        }

        /// <summary>
        /// Save blob to container.
        /// </summary>
        /// <param name="blobName">Blob name.</param>
        /// <param name="stream">Stream of data to save as the blob.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task SaveAsync(string blobName, Stream stream, CancellationToken cancellationToken = default)
        {
            CloudBlockBlob blockBlob = _container.GetBlockBlobReference(blobName);
            
            await blockBlob.UploadFromStreamAsync(stream, cancellationToken);
        }

        /// <summary>
        /// Save text blob to container.
        /// </summary>
        /// <param name="blobName">Blob name.</param>
        /// <param name="data">String data to save as the blob.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task SaveAsync(string blobName, string data, CancellationToken cancellationToken = default)
        {
            await SaveAsync(blobName, data, Encoding.UTF8, cancellationToken);
        }

        /// <summary>
        /// Save text blob to container.
        /// </summary>
        /// <param name="blobName">Blob name.</param>
        /// <param name="data">String data to save as the blob.</param>
        /// <param name="encoding">Text data encoding.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task SaveAsync(string blobName, string data, Encoding encoding, CancellationToken cancellationToken = default)
        {
            using (var stream = StreamHelper.CreateStream(data, encoding))
            {
                await SaveAsync(blobName, stream, cancellationToken);
            }
        }

        /// <summary>
        /// Check if a blob exists in the container.
        /// </summary>
        /// <param name="blobName">Blob name.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<bool> ExistsAsync(string blobName, CancellationToken cancellationToken = default)
        {
            return await _container
                .GetBlockBlobReference(blobName)
                .ExistsAsync(cancellationToken);
        }

        /// <summary>
        /// Get a blob based on it's name.
        /// </summary>
        /// <param name="blobName">Blob name.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<Stream> GetAsStreamAsync(string blobName, CancellationToken cancellationToken = default)
        {
            return await _container
                .GetBlobReference(blobName)
                .OpenReadAsync(cancellationToken);
        }

        /// <summary>
        /// Get a text blob based on it's name.
        /// </summary>
        /// <param name="blobName">Blob name.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<string> GetAsStringAsync(string blobName, CancellationToken cancellationToken = default)
        {
            return await GetAsStringAsync(blobName, Encoding.UTF8, cancellationToken);
        }

        /// <summary>
        /// Get a text blob based on it's name.
        /// </summary>
        /// <param name="blobName">Blob name.</param>
        /// <param name="encoding">Text encoding to use when converting the blob to a string.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="encoding" /> is null.</exception>
        public async Task<string> GetAsStringAsync(string blobName, Encoding encoding, CancellationToken cancellationToken = default)
        {
            using (var stream = await GetAsStreamAsync(blobName, cancellationToken))
            {
                return StreamHelper.ReadStream(stream, encoding);
            }
        }

        /// <summary>
        /// Get all blobs from the container.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<IList<ICloudBlob>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            BlobContinuationToken continuationToken = null;

            var list = new List<ICloudBlob>();

            do
            {
                var resultSegment = await _container.ListBlobsSegmentedAsync(continuationToken, cancellationToken);

                list.AddRange(resultSegment.Results.OfType<ICloudBlob>());

                continuationToken = resultSegment.ContinuationToken;
            } while (continuationToken != null && !cancellationToken.IsCancellationRequested);

            return list;
        }

        /// <summary>
        /// Delete a blob from the container if it exists based on it's name.
        /// </summary>
        /// <param name="blobName">Blob name.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public Task<bool> DeleteAsync(string blobName, CancellationToken cancellationToken = default)
        {
            return _container
                .GetBlockBlobReference(blobName)
                .DeleteIfExistsAsync(cancellationToken);
        }

        /// <summary>
        /// Delete a collection of blobs from the container based on their names. If any blobs do not exist no exception is thrown.
        /// </summary>
        /// <param name="blobNames">Collection of blob names.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="blobNames" /> is null.</exception>
        public async Task DeleteAsync(IEnumerable<string> blobNames, CancellationToken cancellationToken = default)
        {
            await FuncUtils.WhenAllAsync(blob => DeleteAsync(blob, cancellationToken), blobNames);
        }

        /// <summary>
        /// Delete all the blobs in the container.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task DeleteAllAsync(CancellationToken cancellationToken = default)
        {
            BlobContinuationToken continuationToken = null;

            do
            {
                var resultSegment = await _container.ListBlobsSegmentedAsync(continuationToken, cancellationToken);

                await DeleteAsync(resultSegment.Results.OfType<ICloudBlob>().Select(cb => cb.Name), cancellationToken);

                continuationToken = resultSegment.ContinuationToken;
            } while (continuationToken != null && !cancellationToken.IsCancellationRequested);
        }
    }
}