[![Build status](https://ci.appveyor.com/api/projects/status/github/bytedev/ByteDev.Azure.Storage?branch=master&svg=true)](https://ci.appveyor.com/project/bytedev/ByteDev-Azure-Storage/branch/master)
[![NuGet Package](https://img.shields.io/nuget/v/ByteDev.Azure.Storage.svg)](https://www.nuget.org/packages/ByteDev.Azure.Storage)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](https://github.com/ByteDev/ByteDev.Azure.Storage/blob/master/LICENSE)

# ByteDev.Azure.Storage

.NET Standard library of Azure Storage related functionality for Blob, File and Queue.

## Installation

ByteDev.Azure.Storage has been written as a .NET Standard 2.0 library, so you can consume it from a .NET Core or .NET Framework 4.6.1 (or greater) application.

ByteDev.Azure.Storage is hosted as a package on nuget.org.  To install from the Package Manager Console in Visual Studio run:

`Install-Package ByteDev.Azure.Storage`

Further details can be found on the [nuget page](https://www.nuget.org/packages/ByteDev.Azure.Storage/).

## Release Notes

Releases follow semantic versioning.

Full details of the release notes can be viewed on [GitHub](https://github.com/ByteDev/ByteDev.Azure.Storage/blob/master/docs/RELEASE-NOTES.md).

## Usage

### Blob Storage

Blob storage can be accessed through the `BlobStorageClient` class in namespace `ByteDev.Azure.Storage.Blob`.

Methods:

- DeleteAsync
- DeleteAllAsync
- ExistsAsync
- GetAllAsync
- GetAsStreamAsync
- GetAsStringAsync
- SaveAsync

```csharp
IBlobStorageClient client = new BlobStorageClient(connectionString, "MyContainer", true);

await client.SaveAsync("MyBlob", "some text");

string content = await client.GetAsStringAsync("MyBlob");

// content == "some text"

await client.DeleteAsync("MyBlob");
```

---

### Queue Storage

Queue storage can be accessed through the `QueueStorageClient` class in namespace `ByteDev.Azure.Storage.Queue`.

Methods:

- ClearQueueAsync
- DequeueMessageAsync
- EnqueueMessageAsync
- GetMessageCountAsync
- IsQueueEmptyAsync
- PeekMessageAsync

```csharp
IQueueStorageClient client = new QueueStorageClient(connectionString, "MyQueue", true);

await client.EnqueueMessageAsync("test message");

string content = await client.DequeueMessageAsync();

// content == "test message"
```

---

### File Storage

File storage can be accessed through the `FileStorageClient` class in namespace `ByteDev.Azure.Storage.File`.

Methods:

- DeleteIfExistsAsync
- DownloadFileAsync
- DownloadTextAsync
- ExistsAsync
- UploadFileAsync
- UploadStreamAsync
- UploadTextAsync

```csharp
IFileStorageClient client = new FileStorageClient(connectionString, "MyFileShare", true);

string content = await client.DownloadTextAsync("ThisFileExists.txt");
```

