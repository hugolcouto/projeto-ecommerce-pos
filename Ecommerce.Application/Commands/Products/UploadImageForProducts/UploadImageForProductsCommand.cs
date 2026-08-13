using System;

namespace Ecommerce.Application;

public class UploadImageForProductsCommand(Guid idProduct, MemoryStream imageStream, string fileName)
{
    public Guid IdProduct { get; set; } = idProduct;
    public MemoryStream ImageStream { get; set; } = imageStream;
    public string FileName { get; set; } = fileName;
}
