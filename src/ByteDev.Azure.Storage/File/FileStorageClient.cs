using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.File;

namespace ByteDev.Azure.Storage.File
{
    /// <summary>
    /// Represents a client to use Azure File Storage.
    /// </summary>
    public class FileStorageClient : IFileStorageClient
    {
        private readonly CloudFileShare _share;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Azure.Storage.File.FileStorageClient" /> class.
        /// </summary>
        /// <param name="connectionString">Storage account connection string.</param>
        /// <param name="shareName">File share name.</param>
        /// <param name="createShareIfNotExist">True create the share if it does not exist; otherwise do not create.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="connectionString" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="shareName" /> is null.</exception>
        public FileStorageClient(string connectionString, string shareName, bool createShareIfNotExist)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            CloudFileClient fileClient = storageAccount.CreateCloudFileClient();

            _share = fileClient.GetShareReference(shareName);

            if (createShareIfNotExist)
                _share.CreateIfNotExists();
        }

        /// <summary>
        /// Indicates if a given file exists in the share.
        /// </summary>
        /// <param name="cloudFilePath">Cloud file path.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<bool> ExistsAsync(string cloudFilePath, CancellationToken cancellationToken = default)
        {
            var file = GetCloudFile(cloudFilePath);

            return await file.ExistsAsync(cancellationToken);
        }

        /// <summary>
        /// Downloads a file and returns its contents as a string.
        /// </summary>
        /// <param name="cloudFilePath">Cloud file path.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<string> DownloadTextAsync(string cloudFilePath, CancellationToken cancellationToken = default)
        {
            var file = GetCloudFile(cloudFilePath);

            return await file.DownloadTextAsync(cancellationToken);
        }

        /// <summary>
        /// Downloads a file and saves it to a given destination.
        /// </summary>
        /// <param name="cloudFilePath">Cloud file path.</param>
        /// <param name="localFilePath">Local destination file path.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task DownloadFileAsync(string cloudFilePath, string localFilePath, CancellationToken cancellationToken = default)
        {
            var file = GetCloudFile(cloudFilePath);
            
            await file.DownloadToFileAsync(localFilePath, FileMode.CreateNew, cancellationToken);
        }

        /// <summary>
        /// Upload a file to Azure Storage.
        /// </summary>
        /// <param name="localFilePath">Path of file to upload.</param>
        /// <param name="cloudFilePath">Cloud file path.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task UploadFileAsync(string localFilePath, string cloudFilePath, CancellationToken cancellationToken = default)
        {
            var file = GetCloudFile(cloudFilePath);

            var fileInfo = new FileInfo(localFilePath);

            using (FileStream stream = fileInfo.OpenRead())
            {
                await file.UploadFromStreamAsync(stream, cancellationToken);
            }
        }

        /// <summary>
        /// Uploads stream to a Azure Storage file.
        /// </summary>
        /// <param name="stream">Stream of data to upload.</param>
        /// <param name="cloudFilePath">Cloud file path.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task UploadStreamAsync(Stream stream, string cloudFilePath, CancellationToken cancellationToken = default)
        {
            var file = GetCloudFile(cloudFilePath);

            await file.UploadFromStreamAsync(stream, cancellationToken);
        }

        /// <summary>
        /// Uploads text to a Azure Storage file.
        /// </summary>
        /// <param name="content">Text content to upload.</param>
        /// <param name="cloudFilePath">Cloud file path.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task UploadTextAsync(string content, string cloudFilePath, CancellationToken cancellationToken = default)
        {
            var file = GetCloudFile(cloudFilePath);

            await file.UploadTextAsync(content, cancellationToken);
        }

        /// <summary>
        /// Delete a Azure Storage file if it exists.
        /// </summary>
        /// <param name="cloudFilePath">Cloud file path.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task DeleteIfExistsAsync(string cloudFilePath, CancellationToken cancellationToken = default)
        {
            var file = GetCloudFile(cloudFilePath);

            await file.DeleteIfExistsAsync(cancellationToken);
        }

        /// <summary>
        /// Deletes a collection of Azure Storage files if they exist.
        /// </summary>
        /// <param name="cloudFilePaths">Cloud file paths.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="cloudFilePaths" /> is null.</exception>
        public async Task DeleteIfExistsAsync(IEnumerable<string> cloudFilePaths, CancellationToken cancellationToken = default)
        {
            await FuncUtils.WhenAllAsync(file => DeleteIfExistsAsync(file, cancellationToken), cloudFilePaths);
        }

        private CloudFile GetCloudFile(string cloudFilePath)
        {
            CloudFileDirectory rootDir = _share.GetRootDirectoryReference();

            return rootDir.GetFileReference(cloudFilePath);
        }
    }
}