using Ecommerce.Application.Commands.Orders.CreateOrder;
using Ecommerce.Application.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

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
}
