using System.Reflection.Metadata;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Ecommerce.Infrastructure;

public class StorageService : IStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobContainerClient _blobContainerClient;

    public StorageService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient("product-photos");
    }

    public async Task<Stream> DownloadImage(string path)
    {
        BlobClient blobClient = _blobContainerClient.GetBlobClient(path);

        Stream stream = new MemoryStream();

        Azure.Response? response = await blobClient.DownloadToAsync(stream);

        return response.IsError ? throw new Exception($"Error downloading image1 {path}") : stream;
    }

    public async Task<bool> UploadImage(string path, Stream fileStream)
    {
        BlobClient? blobClient = _blobContainerClient.GetBlobClient(path);

        Task<Azure.Response<BlobContentInfo>>? response = blobClient.UploadAsync(fileStream);

        return response is not null;
    }

    public async Task<List<Stream>> DownloadImages(string path)
    {
        List<Stream> streams = [];

        await foreach (
            BlobItem item in _blobContainerClient.GetBlobsAsync(
                prefix: path,
                traits: BlobTraits.None,
                states: BlobStates.None,
                cancellationToken: CancellationToken.None
            )
        )
        {
            BlobClient? blobClient = _blobContainerClient.GetBlobClient(item.Name);

            MemoryStream stream = new();

            Response? response = await blobClient.DownloadToAsync(stream);

            stream.Position = 0;

            streams.Add(stream);
        }

        return streams;
    }
}
