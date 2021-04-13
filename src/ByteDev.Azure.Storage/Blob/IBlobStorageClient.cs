using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Storage.Blob;

namespace ByteDev.Azure.Storage.Blob
{
    public interface IBlobStorageClient
    {
        /// <summary>
        /// Save blob to container.
        /// </summary>
        /// <param name="blobName">Blob name.</param>
        /// <param name="stream">Stream of data to save as the blob.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task SaveAsync(string blobName, Stream stream, CancellationToken cancellationToken = default);

        /// <summary>
        /// Save text blob to container.
        /// </summary>
        /// <param name="blobName">Blob name.</param>
        /// <param name="data">String data to save as the blob.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task SaveAsync(string blobName, string data, CancellationToken cancellationToken = default);

        /// <summary>
        /// Save text blob to container.
        /// </summary>
        /// <param name="blobName">Blob name.</param>
        /// <param name="data">String data to save as the blob.</param>
        /// <param name="encoding">Text data encoding.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task SaveAsync(string blobName, string data, Encoding encoding, CancellationToken cancellationToken = default);

        /// <summary>
        /// Check if a blob exists in the container.
        /// </summary>
        /// <param name="blobName">Blob name.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<bool> ExistsAsync(string blobName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a blob based on it's name.
        /// </summary>
        /// <param name="blobName">Blob name.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<Stream> GetAsStreamAsync(string blobName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a text blob based on it's name.
        /// </summary>
        /// <param name="blobName">Blob name.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<string> GetAsStringAsync(string blobName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a text blob based on it's name.
        /// </summary>
        /// <param name="blobName">Blob name.</param>
        /// <param name="encoding">Text encoding to use when converting the blob to a string.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<string> GetAsStringAsync(string blobName, Encoding encoding, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get all blobs from the container.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<IList<ICloudBlob>> GetAllAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete a blob from the container if it exists based on it's name.
        /// </summary>
        /// <param name="blobName">Blob name.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<bool> DeleteAsync(string blobName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete a collection of blobs from the container based on their names. If any blobs do not exist no exception is thrown.
        /// </summary>
        /// <param name="blobNames">Collection of blob names.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="blobNames" /> is null.</exception>
        Task DeleteAsync(IEnumerable<string> blobNames, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete all the blobs in the container.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task DeleteAllAsync(CancellationToken cancellationToken = default);
    }
}