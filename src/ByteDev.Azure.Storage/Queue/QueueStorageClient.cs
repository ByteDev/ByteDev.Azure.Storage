using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;

namespace ByteDev.Azure.Storage.Queue
{
    /// <summary>
    /// Represents a client to use Azure Queue Storage.
    /// </summary>
    public class QueueStorageClient : IQueueStorageClient
    {
        private readonly CloudQueue _queue;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:ByteDev.Azure.Storage.Queue.QueueStorageClient" /> class.
        /// </summary>
        /// <param name="connectionString">Storage account connection string.</param>
        /// <param name="queueName">Queue name.</param>
        /// <param name="createQueueIfNotExist">True create the queue if it does not exist; otherwise do not create.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="connectionString" /> is null.</exception>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="queueName" /> is null.</exception>
        public QueueStorageClient(string connectionString, string queueName, bool createQueueIfNotExist)
        {
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);

            CloudQueueClient client = account.CreateCloudQueueClient();

            _queue = client.GetQueueReference(queueName);

            if (createQueueIfNotExist)
                _queue.CreateIfNotExists();
        }

        /// <summary>
        /// Enqueue a message.
        /// </summary>
        /// <param name="content">Message content.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task EnqueueMessageAsync(string content, CancellationToken cancellationToken = default)
        {
            var message = new CloudQueueMessage(content);

            await _queue.AddMessageAsync(message, cancellationToken);
        }

        /// <summary>
        /// Dequeue a message and return it's content as a string. If the queue is empty then the returned result will be null.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<string> DequeueMessageAsync(CancellationToken cancellationToken = default)
        {
            CloudQueueMessage message = await _queue.GetMessageAsync(cancellationToken);

            if (message == null)
                return null;

            var content = message.AsString;

            await _queue.DeleteMessageAsync(message, cancellationToken);

            return content;
        }

        /// <summary>
        /// Peek a message and return it's content as a string. If the queue is empty then the returned result will be null.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<string> PeekMessageAsync(CancellationToken cancellationToken = default)
        {
            CloudQueueMessage message = await _queue.PeekMessageAsync(cancellationToken);

            return message?.AsString;
        }

        /// <summary>
        /// Clear the queue of messages.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task ClearQueueAsync(CancellationToken cancellationToken = default)
        {
            CloudQueueMessage message = await _queue.GetMessageAsync(cancellationToken);

            while (message != null)
            {
                await _queue.DeleteMessageAsync(message, cancellationToken);

                message = await _queue.GetMessageAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Indicates if the queue is empty of messages.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<bool> IsQueueEmptyAsync(CancellationToken cancellationToken = default)
        {
            return await _queue.PeekMessageAsync(cancellationToken) == null;
        }

        /// <summary>
        /// Gets the approximate cached message count for the queue.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<int> GetMessageCountAsync(CancellationToken cancellationToken = default)
        {
            await _queue.FetchAttributesAsync(cancellationToken);

            return _queue.ApproximateMessageCount.GetValueOrDefault(0);
        }
    }
}