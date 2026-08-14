// using EcommerceDev.Infrastructure.Caching;
using Ecommerce.Application.Common;
using Ecommerce.Application.Queries.Products.GetAllProducts;
using Ecommerce.Core.Entities;
using Ecommerce.Core.Repositories;
using Ecommerce.Infrastructure.Caching;
using Moq;

namespace Ecommerce.UnitTests.Application;

public class GetAllProductsQueryTests
{
    // Scenario 01: 3 products not in cache
    [Fact]
    public async Task ThreeProductsNotInCache_GetAllProductsIsCalled_ReturnCorrectValues()
    {
        // Arrange
        var productRepositoryMock = new Mock<IProductRepository>();
        var cacheServiceMock = new Mock<ICacheService>();

        var items = new List<Product>
        {
            new("Product 1", "Description 1", 1, "Brand 1", 1, Guid.NewGuid()),
            new("Product 2", "Description 2", 1, "Brand 2", 2, Guid.NewGuid()),
            new("Product 3", "Description 3", 1, "Brand 3", 3, Guid.NewGuid()),
        };

        productRepositoryMock.Setup(pr => pr.GetAll()).ReturnsAsync(items);

        // Act
        var query = new GetAllProductsQuery();
        var handler = new GetAllProductsQueryHandler(
            productRepositoryMock.Object,
            cacheServiceMock.Object
        );

        ResultViewModel<List<GetAllProductsItemViewModel>>? result = await handler.HandleAsync(
            query
        );

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.NotEmpty(result.Data);
        cacheServiceMock.Verify(
            pr => pr.GetAsync<List<GetAllProductsItemViewModel>>(It.IsAny<string>()),
            Times.Once
        );
        productRepositoryMock.Verify(pr => pr.GetAll(), Times.Once);
    }

    // Scenario 02: 3 products in cache
    [Fact]
    public async Task ThreeProductsInCache_GetAllProductsIsCalled_ReturnCorrectValues()
    {
        // Arrange
        var productRepositoryMock = new Mock<IProductRepository>();
        var cacheServiceMock = new Mock<ICacheService>();

        var items = new List<GetAllProductsItemViewModel>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product A",
                Price = 1,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product B",
                Price = 2,
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Product C",
                Price = 3,
            },
        };

        cacheServiceMock
            .Setup(pr => pr.GetAsync<List<GetAllProductsItemViewModel>>(It.IsAny<string>()))
            .ReturnsAsync(items);

        // Act
        var query = new GetAllProductsQuery();
        var handler = new GetAllProductsQueryHandler(
            productRepositoryMock.Object,
            cacheServiceMock.Object
        );

        ResultViewModel<List<GetAllProductsItemViewModel>>? result = await handler.HandleAsync(
            query
        );

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.NotEmpty(result.Data);
        cacheServiceMock.Verify(
            pr => pr.GetAsync<List<GetAllProductsItemViewModel>>(It.IsAny<string>()),
            Times.Once
        );
        productRepositoryMock.Verify(pr => pr.GetAll(), Times.Never);
    }
}
