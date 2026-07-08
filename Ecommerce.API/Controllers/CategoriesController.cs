using Ecommerce.Application.Commands.ProductsCommands.Categories.CreateCategory;
using Ecommerce.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCategoryCommand request)
    {
        ResultViewModel<Guid>? result = await _mediator.DispatchAsync<
            CreateCategoryCommand,
            ResultViewModel<Guid>
        >(request);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result);
    }
}
