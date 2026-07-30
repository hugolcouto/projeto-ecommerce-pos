using Ecommerce.Application.Commands.ShoppingCarts.CreateOrUpdateShoppingCart;
using Ecommerce.Application.Common;
using Ecommerce.Application.Common.ShoppingCart;
using Ecommerce.Application.Queries.ShoppingCarts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ShoppingCartsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShoppingCartsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{idCustomer:guid}")]
    public async Task<IActionResult> Get(Guid idCustomer)
    {
        Console.WriteLine($"Chegou aqui");
        GetShoppingCartQuery query = new(idCustomer);

        ResultViewModel<List<ProductItemShoppingCartModel>>? result = await _mediator.DispatchAsync<
            GetShoppingCartQuery,
            ResultViewModel<List<ProductItemShoppingCartModel>>
        >(query);

        if (result.IsSuccess is false)
            return BadRequest(result.Message);

        return Ok(result.Data);
    }

    [HttpPut("{idCustomer:guid}")]
    public async Task<IActionResult> Put(Guid idCustomer, CreateOrUpdateShoppingCartCommand command)
    {
        command.IdCustomer = idCustomer;

        GetShoppingCartQuery query = new(idCustomer);

        ResultViewModel<bool>? result = await _mediator.DispatchAsync<
            CreateOrUpdateShoppingCartCommand,
            ResultViewModel<bool>
        >(command);

        if (result.IsSuccess is false)
            return BadRequest(result.Message);

        return Ok(result.Data);
    }
}
