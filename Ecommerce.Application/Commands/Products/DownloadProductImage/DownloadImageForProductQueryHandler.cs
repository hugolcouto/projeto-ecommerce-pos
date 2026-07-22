using System.Net;
using Ecommerce.Application.Common;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Repositories;
using Ecommerce.Infrastructure;

namespace Ecommerce.Application.Commands.Products.DownloadProductImage;

public class DownloadImageForProductQueryHandler
    : IHandler<DownloadImageForProductQuery, ResultViewModel<Stream>>
{
    private readonly IStorageService _storageService;
    private readonly IProductRepository _productRepository;

    public DownloadImageForProductQueryHandler(
        IStorageService storageService,
        IProductRepository productRepository
    )
    {
        _storageService = storageService;
        _productRepository = productRepository;
    }

    public async Task<ResultViewModel<Stream>> HandleAsync(DownloadImageForProductQuery request)
    {
        ProductImage? productImage = await _productRepository.GetImageById(request.IdProductImage);

        if (productImage is null)
            return ResultViewModel<Stream>.Error("Image not found", HttpStatusCode.NotFound, null);

        Stream? stream;

        try
        {
            stream = await _storageService.DownloadImage(productImage.Path);
        }
        catch (Exception ex)
        {
            return ResultViewModel<Stream>.Error(
                $"Error downloading image {ex.Message}",
                HttpStatusCode.InternalServerError,
                null
            );
        }

        if (stream is null)
            return ResultViewModel<Stream>.Error(
                "Image stream is null",
                HttpStatusCode.NotFound,
                null
            );

        stream.Position = 0;
        return ResultViewModel<Stream>.Success(stream);
    }
}
