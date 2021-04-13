using System;
using ByteDev.Azure.Storage.Queue;
using NUnit.Framework;

namespace ByteDev.Azure.Storage.UnitTests.Queue
{
    [TestFixture]
    public class QueueStorageClientTests
    {
        [TestFixture]
        public class Constructor : QueueStorageClientTests
        {
            private const string QueueName = "MyQueue";

            [Test]
            public void WhenConnectionStringIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => _ = new QueueStorageClient(null, QueueName, false));
            }

            [Test]
            public void WhenQueueNameIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => _ = new QueueStorageClient(TestConnectionStrings.Valid, null, false));
            }
        }
    }
}