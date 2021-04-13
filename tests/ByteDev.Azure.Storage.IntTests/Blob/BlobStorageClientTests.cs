using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ByteDev.Azure.Storage.Blob;
using ByteDev.Collections;
using NUnit.Framework;

namespace ByteDev.Azure.Storage.IntTests.Blob
{
    [TestFixture]
    [NonParallelizable]
    public class BlobStorageClientTests : BlobTestBase
    {
        private BlobStorageClient _sut;

        private const string ContainerName = "testblobs";

        public BlobStorageClientTests() : base(ContainerName)
        {
        }

        [SetUp]
        public void SetUp()
        {
            _sut = new BlobStorageClient(ConnectionString, ContainerName, true);
        }

        [OneTimeTearDown]
        public void ClassTearDown()
        {
            CleanUpBlobs();
        }

        [TestFixture]
        public class ExistsAsync : BlobStorageClientTests
        {
            [Test]
            public async Task WhenBlobDoesNotExist_ThenReturnFalse()
            {
                var result = await _sut.ExistsAsync(BlobNameFactory.Create());

                Assert.That(result, Is.False);
            }

            [Test]
            public async Task WhenBlobExists_ThenReturnTrue()
            {
                var blobName = await SaveBlobAsync("exists", "123");

                var result = await _sut.ExistsAsync(blobName);

                Assert.That(result, Is.True);
            }
        }

        [TestFixture]
        public class DeleteAsync : BlobStorageClientTests
        {
            [Test]
            public async Task WhenBlobDoesNotExist_ThenReturnFalse()
            {
                var result = await _sut.DeleteAsync(BlobNameFactory.Create());

                Assert.That(result, Is.False);
            }

            [Test]
            public async Task WhenBlobExists_ThenDeleteBlob()
            {
                var blobName = await SaveBlobAsync("delete", "123");

                var result = await _sut.DeleteAsync(blobName);

                Assert.That(result, Is.True);
            }

            [Test]
            public async Task WhenDeletingMultipleBlobs_ThenDeleteBlobs()
            {
                var blobName1 = await SaveBlobAsync("delete", "123");
                var blobName2 = await SaveBlobAsync("delete", "123");

                var list = new List<string> {blobName1, blobName2};

                await _sut.DeleteAsync(list);

                Assert.That(await _sut.ExistsAsync(blobName1), Is.False);
                Assert.That(await _sut.ExistsAsync(blobName2), Is.False);
            }
        }

        [TestFixture]
        public class SaveAsync : BlobStorageClientTests
        {
            private const string Data = "save123";

            [Test]
            public async Task WhenStreamHasData_ThenSaveBlob()
            {
                string blobName = BlobNameFactory.Create("save");

                var stream = StreamHelper.CreateStream(Data);

                await _sut.SaveAsync(blobName, stream);

                TrackBlob(blobName);

                var result = await _sut.ExistsAsync(blobName);

                Assert.That(result, Is.True);
            }

            [Test]
            public async Task WhenStringData_ThenSaveBlob()
            {
                string blobName = BlobNameFactory.Create("save");

                await _sut.SaveAsync(blobName, Data);

                TrackBlob(blobName);

                var result = await _sut.ExistsAsync(blobName);

                Assert.That(result, Is.True);
            }
        }

        [TestFixture]
        public class GetAsStreamAsync : BlobStorageClientTests
        {
            private const string Data = "get123";

            [Test]
            public async Task WhenCalled_ThenReturnsBlobStream()
            {
                var name = await SaveBlobAsync("get", Data);

                var result = await _sut.GetAsStreamAsync(name);

                var readStr = StreamHelper.ReadStream(result);

                Assert.That(readStr, Is.EqualTo(Data));
            }
        }

        [TestFixture]
        public class GetAsStringAsync : BlobStorageClientTests
        {
            private const string Data = "getasstring123";

            [Test]
            public async Task WhenCalled_ThenReturnsBlobStringContents()
            {
                var name = await SaveBlobAsync("get", Data);

                var result = await _sut.GetAsStringAsync(name);
                
                Assert.That(result, Is.EqualTo(Data));
            }
        }

        [TestFixture]
        public class GetAllAsync : BlobStorageClientTests
        {
            [Test]
            public async Task WhenBlobsExists_ThenReturnBlobs()
            {
                var blobName1 = await SaveBlobAsync("list", "123");
                var blobName2 = await SaveBlobAsync("list", "456");

                var result = await _sut.GetAllAsync();

                Assert.That(result.Count, Is.EqualTo(2));
                Assert.That(result.First().Name, Is.EqualTo(blobName1).Or.EqualTo(blobName2));
                Assert.That(result.Second().Name, Is.EqualTo(blobName1).Or.EqualTo(blobName2));
            }
        }

        [TestFixture]
        public class DeleteAllAsync : BlobStorageClientTests
        {
            [Test]
            public async Task WhenBlobsExist_ThenDeleteBlobs()
            {
                await SaveBlobAsync("list", "123");
                await SaveBlobAsync("list", "456");
                await SaveBlobAsync("somePath/list", "789");

                await _sut.DeleteAllAsync();

                var blobs = await _sut.GetAllAsync();

                Assert.That(blobs, Is.Empty);
            }

            [Test]
            public async Task WhenContainerIsEmpty_ThenDoNothing()
            {
                await _sut.DeleteAllAsync();

                await _sut.DeleteAllAsync();

                var blobs = await _sut.GetAllAsync();

                Assert.That(blobs, Is.Empty);
            }
        }
    }
}