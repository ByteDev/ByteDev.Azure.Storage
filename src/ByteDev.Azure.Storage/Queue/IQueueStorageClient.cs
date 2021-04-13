using System.Threading;
using System.Threading.Tasks;

namespace ByteDev.Azure.Storage.Queue
{
    public interface IQueueStorageClient
    {
        /// <summary>
        /// Enqueue a message.
        /// </summary>
        /// <param name="content">Message content.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task EnqueueMessageAsync(string content, CancellationToken cancellationToken = default);

        /// <summary>
        /// Dequeue a message and return it's content as a string. If the queue is empty then the returned result will be null.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<string> DequeueMessageAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Peek a message and return it's content as a string. If the queue is empty then the returned result will be null.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<string> PeekMessageAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Clear the queue of messages.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task ClearQueueAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Indicates if the queue is empty of messages.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<bool> IsQueueEmptyAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the approximate cached message count for the queue.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<int> GetMessageCountAsync(CancellationToken cancellationToken = default);
    }
}