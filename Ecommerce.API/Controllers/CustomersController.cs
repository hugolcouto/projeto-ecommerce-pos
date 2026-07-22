using Ecommerce.Application.Commands.ProductsCommands.Customers.CreateCostumer;
using Ecommerce.Application.Commands.ProductsCommands.Customers.CreateCostumerAddresses;
using Ecommerce.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers.v1;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer(CreateCustomerCommand request)
    {
        ResultViewModel<Guid>? result = await _mediator.DispatchAsync<
            CreateCustomerCommand,
            ResultViewModel<Guid>
        >(request);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result);
    }

    [HttpPost("{idCustomer}/addresses")]
    public async Task<IActionResult> CreateAddress(CreateCustomerAddressCommand request)
    {
        ResultViewModel<Guid>? result = await _mediator.DispatchAsync<
            CreateCustomerAddressCommand,
            ResultViewModel<Guid>
        >(request);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result);
    }
}
