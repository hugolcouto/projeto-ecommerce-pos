using Ecommerce.Core.Entities;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SeedsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> SeedData([FromServices] EcommerceDbContext context)
    {
        Customer customer = new(
            fullName: "Jon Doe",
            email: "contato@email.com",
            phoneNumber: "99123-1234",
            document: "123.456.789-00",
            birthDate: DateTimeOffset.Now.AddYears(-35)
        );

        CustomerAddress customerAddress = new(
            idCustomer: customer.Id,
            street: "Rua das Couves",
            addressLine2: "Bairro Barro, 100",
            zipCode: "23900-000",
            addressLine1: "Rua das Couves",
            district: "Bairro Barro",
            city: "Angra dos Reis",
            state: "Rio de Janeiro",
            recipientName: "Jon Doe"
        );

        ProductCategory category = new(title: "Hardware", subCategory: "Computer");

        Product product = new(
            title: "RX9060XT",
            description: "AMD GPU",
            price: 3500.0m,
            brand: "AMD",
            quantity: 1,
            idCategory: category.Id
        );

        Order order = new(
            idCustomer: customer.Id,
            deliveryAddressId: customerAddress.Id,
            shippingPrice: 10,
            totalPrice: product.Price * 2,
            items: [new(idProduct: product.Id, quantity: 2, price: product.Price)]
        );

        var objects = new List<object> { customer, customerAddress, category, product, order };

        await context.AddRangeAsync(objects);

        await context.SaveChangesAsync();

        return Ok(product.Id);
    }
}
