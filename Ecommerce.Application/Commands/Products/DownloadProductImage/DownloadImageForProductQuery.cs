using System;

namespace Ecommerce.Application.Commands.Products.DownloadProductImage;

public class DownloadImageForProductQuery
{
    public DownloadImageForProductQuery(Guid idProductImage)
    {
        IdProductImage = idProductImage;
    }

    public Guid IdProductImage { get; set; }
}
