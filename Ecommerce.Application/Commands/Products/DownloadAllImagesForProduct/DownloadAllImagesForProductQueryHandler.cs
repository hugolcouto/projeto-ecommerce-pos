using System.Net;
using Ecommerce.Application.Commands.Products.DownloadImageForProduct;
using Ecommerce.Application.Common;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Repositories;
using Ecommerce.Infrastructure;
using Microsoft.VisualBasic;

namespace Ecommerce.Application.Commands.Products.DownloadAllImagesForProduct;

public class DownloadAllImagesForProductQueryHandler
    : IHandler<DownloadAllImagesForProductQuery, ResultViewModel<List<Stream>>>
{
    private readonly IStorageService _storageService;

    public DownloadAllImagesForProductQueryHandler(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task<ResultViewModel<List<Stream>>> HandleAsync(
        DownloadAllImagesForProductQuery request
    )
    {
        List<Stream> streams = await _storageService.DownloadImages($"{request.IdProduct}/");

        return ResultViewModel<List<Stream>>.Success(streams);
    }
}
