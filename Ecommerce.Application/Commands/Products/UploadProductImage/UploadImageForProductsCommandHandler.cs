using Ecommerce.Application.Common;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Repositories;
using Ecommerce.Infrastructure;

namespace Ecommerce.Application;

public class UploadImageForProductsCommandHandler
    : IHandler<UploadImageForProductsCommand, ResultViewModel<bool>>
{
    private readonly IStorageService _storageService;
    private readonly IProductRepository _productRepository;

    public UploadImageForProductsCommandHandler(
        IStorageService storageService,
        IProductRepository productRepository
    )
    {
        _storageService = storageService;
        _productRepository = productRepository;
    }

    public async Task<ResultViewModel<bool>> HandleAsync(UploadImageForProductsCommand request)
    {
        string fileExtension = request.FileName.Split('.').Last();

        ProductImage productImage = new(true, request.IdProduct);
        productImage.ConfigureIdentifier(fileExtension);

        await _storageService.UploadImage(productImage.Path, request.ImageStream);

        await _productRepository.CreateImage(productImage);
        return ResultViewModel<bool>.Success(true);
    }
}
