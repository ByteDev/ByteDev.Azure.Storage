using System.Threading.Tasks;
using ByteDev.Azure.Storage.File;
using ByteDev.Testing.NUnit;
using Microsoft.Azure.Storage;
using NUnit.Framework;

namespace ByteDev.Azure.Storage.IntTests.File
{
    [TestFixture]
    [NonParallelizable]
    public class FileStorageClientTests : FileTestBase
    {
        private const string ShareName = "testfileshare";

        private const string DoesNotExistFile = "doesnotexist.txt";

        private FileStorageClient _sut;

        public FileStorageClientTests() : base(ShareName)
        {
        }

        [SetUp]
        public void SetUp()
        {
            var connString = GetTestConnectionString();

            _sut = new FileStorageClient(connString, ShareName, true);
        }

        [TearDown]
        public async Task TearDown()
        {
            await CleanUpFilesAsync();
        }

        [TestFixture]
        public class ExistsAsync : FileStorageClientTests
        {
            [Test]
            public async Task WhenFileDoesNotExist_ThenReturnFalse()
            {
                var result = await _sut.ExistsAsync(DoesNotExistFile);

                Assert.That(result, Is.False);
            }

            [Test]
            public async Task WhenFileExistsInRoot_ThenReturnTrue()
            {
                const string cloudFilePath = "ExistsAsyncTest1.txt";

                await CreateCloudFileAsync(cloudFilePath);

                var result = await _sut.ExistsAsync(cloudFilePath);

                Assert.That(result, Is.True);
            }

            [Test]
            public async Task WhenFileExistsInSubFolder_ThenReturnTrue()
            {
                const string cloudFilePath = "TestFiles/ExistsAsyncTest2.txt";

                await CreateCloudFileAsync(cloudFilePath);

                var result = await _sut.ExistsAsync(cloudFilePath);

                Assert.That(result, Is.True);
            }
        }

        [TestFixture]
        public class DownloadTextAsync : FileStorageClientTests
        {
            [Test]
            public void WhenFileDoesNotExist_ThenThrowException()
            {
                Assert.ThrowsAsync<StorageException>(() => _sut.DownloadTextAsync(DoesNotExistFile));
            }

            [Test]
            public async Task WhenFileExistsInRoot_ThenReturnContentsAsString()
            {
                const string cloudFilePath = "DownloadTextAsync1.txt";

                var content = await CreateCloudFileAsync(cloudFilePath);

                var result = await _sut.DownloadTextAsync(cloudFilePath);

                Assert.That(result, Is.EqualTo(content));
            }
        }

        [TestFixture]
        public class DownloadFileAsync : FileStorageClientTests
        {
            private const string DestinationFilePath = @"C:\Temp\testfile1.txt";

            [Test]
            public async Task WhenFileExistsInRoot_ThenDownloadFile()
            {
                DeleteLocalFileIfExists(DestinationFilePath);

                const string cloudFilePath = "DownloadTextAsync1.txt";

                await CreateCloudFileAsync(cloudFilePath);

                await _sut.DownloadFileAsync(cloudFilePath, DestinationFilePath);

                AssertFile.Exists(DestinationFilePath);
            }
        }

        [TestFixture]
        public class UploadFileAsync : FileStorageClientTests
        {
            private const string LocalTestFile1 = @"File\LocalTextFile1.txt";

            [Test]
            public async Task WhenFileDoesNotExistInCloud_ThenUploadFile()
            {
                const string cloudFilePath = @"UploadFileAsyncTest1.txt";

                await DeleteIfExistsAsync(cloudFilePath);

                await _sut.UploadFileAsync(LocalTestFile1, cloudFilePath);

                await AssertCloudFileExistsAsync(cloudFilePath);
            }
        }

        [TestFixture]
        public class UploadTextAsync : FileStorageClientTests
        {
            [Test]
            public async Task WhenFileDoesNotExistInCloud_ThenUploadFile()
            {
                const string content = "this is a upload text test";
                const string cloudFilePath = @"UploadTextAsyncTest1.txt";

                await DeleteIfExistsAsync(cloudFilePath);

                await _sut.UploadTextAsync(content, cloudFilePath);

                await AssertCloudFileTextEqualsAsync(cloudFilePath, content);
            }
        }
    }
}