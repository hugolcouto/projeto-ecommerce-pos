using System;

namespace Ecommerce.Application;

public class UploadImageForProductsCommand
{
    public UploadImageForProductsCommand(Guid idProduct, MemoryStream imageStream, string fileName)
    {
        IdProduct = idProduct;
        ImageStream = imageStream;
        FileName = fileName;
    }

    public Guid IdProduct { get; set; }
    public MemoryStream ImageStream { get; set; }
    public string FileName { get; set; }
}
