using Ecommerce.Application;
using Ecommerce.Application.Commands.Orders.CreateOrder;
using Ecommerce.Application.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class OrdersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderCommand request)
    {
        ResultViewModel<Guid>? result = await _mediator.DispatchAsync<
            CreateOrderCommand,
            ResultViewModel<Guid>
        >(request);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result);
    }

    [HttpPost("/shipping")]
    public async Task<IActionResult> CalculateShipping(CalculateShippingQuery request)
    {
        ResultViewModel<decimal>? result = await _mediator.DispatchAsync<
            CalculateShippingQuery,
            ResultViewModel<decimal>
        >(request);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result);
    }
}
