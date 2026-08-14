using Ecommerce.Core.Entities;
using Ecommerce.Core.Repositories;
using Ecommerce.Core.Services;
using Moq;

namespace Ecommerce.UnitTests.Core;

// PKM: 30
// PPU: 2.5
// Formula: (PKM * DKM) + ((PPU * QTD) * ITM)
public class OrderDomainServiceTests
{
    // Should pass if distance in km is 100 with 5 units is equals to 3012.50
    [Fact]
    public void Distance100KmAnd5Units_CalculateShippingCostIsCalled_ReturnCorrectValue()
    {
        // Arrange
        Mock<IProductRepository> repositoryMock = new();
        const int distanceInKm = 100;
        const int quantity = 5;
        List<OrderItem> orderItems = [new OrderItem(Guid.NewGuid(), quantity)];

        // Act
        OrderDomainService orderDomainService = new(repositoryMock.Object);
        decimal result = orderDomainService.CalculateShippingCost(distanceInKm, orderItems);

        // Assert
        Assert.Equal(3_012.50m, result);
    }

    // <summary>
    // Should pass if distance in km is 0 with 10 units is equals to 55
    // </summary>
    [Fact]
    public void Distance0KmAnd10Units_CalculateShippingCostIsCalled_ReturnCorrectValue()
    {
        // Arrange
        Mock<IProductRepository> repositoryMock = new();
        const int distanceInKm = 0;
        const int quantity = 10;
        List<OrderItem> orderItems = [new OrderItem(Guid.NewGuid(), quantity)];

        // Act
        OrderDomainService orderDomainService = new(repositoryMock.Object);
        decimal result = orderDomainService.CalculateShippingCost(distanceInKm, orderItems);

        // Assert
        Assert.Equal(55m, result);
    }

    // Should pass if distance in km is 50 with 2 items with 7 units is equals to 55
    [Fact]
    public void Distance50KmAnd7Units_CalculateShippingCostIsCalled_ReturnCorrectValue()
    {
        // Arrange
        Mock<IProductRepository> repositoryMock = new();
        const int distanceInKm = 50;
        const int quantity = 7;
        List<OrderItem> orderItems =
        [
            new OrderItem(Guid.NewGuid(), quantity),
            new OrderItem(Guid.NewGuid(), quantity),
        ];

        // Act
        OrderDomainService orderDomainService = new(repositoryMock.Object);
        decimal result = orderDomainService.CalculateShippingCost(distanceInKm, orderItems);

        // Assert
        Assert.Equal(1535m, result);
    }

    // Should fail if has no items
    [Fact]
    public void Distance20Km0Units_CalculateShippingCostIsCalled_ThrowError()
    {
        // Arrange
        Mock<IProductRepository> repositoryMock = new();
        const int distanceInKm = 50;
        List<OrderItem> orderItems = [];
        Console.WriteLine(orderItems.Count);

        // Act
        OrderDomainService orderDomainService = new(repositoryMock.Object);
        decimal action() => orderDomainService.CalculateShippingCost(distanceInKm, orderItems);

        // Assert
        InvalidOperationException exeption = Assert.Throws<InvalidOperationException>(() =>
            action()
        );
        Assert.Equal("No items found in cart", exeption.Message);
    }

    // Should fail in 500km and 10 items
    [Fact]
    public void Distance500KmAnd10items_CalculateShippingCostIsCalled_ThrowError()
    {
        // Arrange
        Mock<IProductRepository> repositoryMock = new();
        const int distanceInKm = 500;
        const int quantity = 10;
        List<OrderItem> orderItems = [new OrderItem(Guid.NewGuid(), quantity)];
        Console.WriteLine(orderItems.Count);

        // Act
        OrderDomainService orderDomainService = new(repositoryMock.Object);
        decimal action() => orderDomainService.CalculateShippingCost(distanceInKm, orderItems);

        // Assert
        ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            action()
        );
        Assert.StartsWith("Above maximum accepted value", exception.Message);
        // Assert.Equal("Above maximum accepted value", exception.Message);
    }
}
