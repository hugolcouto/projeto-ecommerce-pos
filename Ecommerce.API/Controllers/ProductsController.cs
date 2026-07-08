using Ecommerce.Application.Commands.ProductsCommands.Products.CreateProduct;
using Ecommerce.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductCommand request)
    {
        ResultViewModel<Guid>? result = await _mediator.DispatchAsync<
            CreateProductCommand,
            ResultViewModel<Guid>
        >(request);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result);
    }
}
