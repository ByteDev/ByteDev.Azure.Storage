using System;
using ByteDev.Azure.Storage.Blob;
using NUnit.Framework;

namespace ByteDev.Azure.Storage.UnitTests.Blob
{
    [TestFixture]
    public class BlobStorageClientTests
    {
        [TestFixture]
        public class Constructor : BlobStorageClientTests
        {
            private const string ContainerName = "MyContainer";

            [Test]
            public void WhenConnectionStringIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => _ = new BlobStorageClient(null, ContainerName, false));
            }

            [Test]
            public void WhenQueueNameIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => _ = new BlobStorageClient(TestConnectionStrings.Valid, null, false));
            }
        }
    }
}