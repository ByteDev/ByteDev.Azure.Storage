using System;
using ByteDev.Azure.Storage.File;
using NUnit.Framework;

namespace ByteDev.Azure.Storage.UnitTests.File
{
    [TestFixture]
    public class FileStorageClientTests
    {
        [TestFixture]
        public class Constructor : FileStorageClientTests
        {
            private const string ShareName = "MyShare";

            [Test]
            public void WhenConnectionStringIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => _ = new FileStorageClient(null, ShareName, false));
            }

            [Test]
            public void WhenQueueNameIsNull_ThenThrowException()
            {
                Assert.Throws<ArgumentNullException>(() => _ = new FileStorageClient(TestConnectionStrings.Valid, null, false));
            }
        }
    }
}