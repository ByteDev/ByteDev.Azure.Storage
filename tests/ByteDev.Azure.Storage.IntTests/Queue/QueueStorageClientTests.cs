using System.Threading.Tasks;
using ByteDev.Azure.Storage.Queue;
using NUnit.Framework;

namespace ByteDev.Azure.Storage.IntTests.Queue
{
    [TestFixture]
    [NonParallelizable]
    public class QueueStorageClientTests : IntTestBase
    {
        private const string QueueName = "testqueue";

        private QueueStorageClient _sut;

        [SetUp]
        public async Task SetUp()
        {
            /*
            A queue name 
            - must start with a letter or number, and can only contain letters, numbers, and the dash (-) character.
            - The first and last letters in the queue name must be alphanumeric. ...
            - All letters in a queue name must be lowercase.
            - A queue name must be from 3 through 63 characters long
            */

            var connString = GetTestConnectionString();

            _sut = new QueueStorageClient(connString, QueueName, true);

            await _sut.ClearQueueAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            await _sut.ClearQueueAsync();
        }

        [TestFixture]
        public class EnqueueMessageAsync : QueueStorageClientTests
        {
            [TestCase("")]
            [TestCase("test message")]
            public async Task WhenCalled_ThenAddsMessageToQueue(string content)
            {
                await _sut.EnqueueMessageAsync(content);

                var result = await _sut.DequeueMessageAsync();

                Assert.That(result, Is.EqualTo(content));
            }

            [Test]
            public async Task WhenContentIsNull_ThenAddsEmptyContentToQueue()
            {
                await _sut.EnqueueMessageAsync(null);

                var result = await _sut.DequeueMessageAsync();

                Assert.That(result, Is.Empty);
            }
        }

        [TestFixture]
        public class DequeueMessageAsync : QueueStorageClientTests
        {
            [Test]
            public async Task WhenQueueIsEmpty_ThenReturnNull()
            {
                var result = await _sut.DequeueMessageAsync();

                Assert.That(result, Is.Null);
            }

            [Test]
            public async Task WhenQueueHasMessage_ThenReturnsMessage()
            {
                await _sut.EnqueueMessageAsync("some test message");

                var result = await _sut.DequeueMessageAsync();

                Assert.That(result, Is.EqualTo("some test message"));
            }
        }

        [TestFixture]
        public class PeekMessageAsync : QueueStorageClientTests
        {
            [Test]
            public async Task WhenQueueIsEmpty_ThenReturnNull()
            {
                var result = await _sut.PeekMessageAsync();

                Assert.That(result, Is.Null);
            }

            [Test]
            public async Task WhenQueueHasMessage_ThenReturnsMessageAndNoDelete()
            {
                const string message = "test message";

                await _sut.EnqueueMessageAsync(message);

                var result1 = await _sut.PeekMessageAsync();

                Assert.That(result1, Is.EqualTo(message));

                var result2 = await _sut.DequeueMessageAsync();
                Assert.That(result2, Is.EqualTo(message));
            }
        }

        [TestFixture]
        public class ClearQueueAsync : QueueStorageClientTests
        {
            [Test]
            public async Task WhenQueueIsEmpty_ThenDoNothing()
            {
                await _sut.ClearQueueAsync();

                var result = await _sut.IsQueueEmptyAsync();

                Assert.That(result, Is.True);
            }

            [Test]
            public async Task WhenQueueHasMessages_ThenEmptyQueue()
            {
                await _sut.EnqueueMessageAsync("message 1");
                await _sut.EnqueueMessageAsync("message 2");

                await _sut.ClearQueueAsync();

                var result = await _sut.IsQueueEmptyAsync();

                Assert.That(result, Is.True);
            }
        }

        [TestFixture]
        public class IsQueueEmptyAsync : QueueStorageClientTests
        {
            [Test]
            public async Task WhenIsEmpty_ThenReturnTrue()
            {
                var result = await _sut.IsQueueEmptyAsync();

                Assert.That(result, Is.True);
            }

            [Test]
            public async Task WhenIsNotEmpty_ThenReturnFalse()
            {
                await _sut.EnqueueMessageAsync("message");

                var result = await _sut.IsQueueEmptyAsync();

                Assert.That(result, Is.False);
            }
        }

        [TestFixture]
        public class GetMessageCountAsync : QueueStorageClientTests
        {
            [Test]
            public async Task WhenQueueIsEmpty_ThenReturnZero()
            {
                var result = await _sut.GetMessageCountAsync();

                Assert.That(result, Is.EqualTo(0));
            }

            [Test]
            public async Task WhenQueueIsNotEmpty_ThenReturnGreaterThanZero()
            {
                await _sut.EnqueueMessageAsync("message 1");
                await _sut.EnqueueMessageAsync("message 2");

                var result = await _sut.GetMessageCountAsync();

                Assert.That(result, Is.GreaterThan(0));
            }
        }
    }
}