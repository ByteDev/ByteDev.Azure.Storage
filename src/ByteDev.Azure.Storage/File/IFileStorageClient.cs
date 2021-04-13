using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ByteDev.Azure.Storage.File
{
    public interface IFileStorageClient
    {
        /// <summary>
        /// Indicates if a given file exists in the share.
        /// </summary>
        /// <param name="cloudFilePath">Cloud file path.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<bool> ExistsAsync(string cloudFilePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads a file and returns its contents as a string.
        /// </summary>
        /// <param name="cloudFilePath">Cloud file path.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<string> DownloadTextAsync(string cloudFilePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads a file and saves it to a given destination.
        /// </summary>
        /// <param name="cloudFilePath">Cloud file path.</param>
        /// <param name="localFilePath">Local destination file path.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task DownloadFileAsync(string cloudFilePath, string localFilePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Upload a file to Azure Storage.
        /// </summary>
        /// <param name="localFilePath">Path of file to upload.</param>
        /// <param name="cloudFilePath">Cloud file path.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task UploadFileAsync(string localFilePath, string cloudFilePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads stream to a Azure Storage file.
        /// </summary>
        /// <param name="stream">Stream of data to upload.</param>
        /// <param name="cloudFilePath">Cloud file path.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task UploadStreamAsync(Stream stream, string cloudFilePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads text to a Azure Storage file.
        /// </summary>
        /// <param name="content">Text content to upload.</param>
        /// <param name="cloudFilePath">Cloud file path.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task UploadTextAsync(string content, string cloudFilePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete a Azure Storage file if it exists.
        /// </summary>
        /// <param name="cloudFilePath">Cloud file path.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task DeleteIfExistsAsync(string cloudFilePath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a collection of Azure Storage files if they exist.
        /// </summary>
        /// <param name="cloudFilePaths">Cloud file paths.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task DeleteIfExistsAsync(IEnumerable<string> cloudFilePaths, CancellationToken cancellationToken = default);
    }
}