using System;

namespace Ecommerce.Infrastructure;

public interface IStorageService
{
    Task<bool> UploadImage(string path, Stream fileStream);
    Task<Stream> DownloadImage(string path);
}
