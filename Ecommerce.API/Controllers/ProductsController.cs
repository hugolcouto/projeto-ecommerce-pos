using System.Net;
using System.Security.Cryptography.X509Certificates;
using Ecommerce.Application;
using Ecommerce.Application.Commands.Products.DownloadProductImage;
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

    [HttpPost("{id:guid}/photos")]
    public async Task<IActionResult> UploadPhoto(Guid id, IFormFile file)
    {
        MemoryStream? stream = new();
        await file.CopyToAsync(stream);

        stream.Position = 0;

        var command = new UploadImageForProductsCommand(
            idProduct: id,
            imageStream: stream,
            fileName: file.FileName
        );

        ResultViewModel<bool>? response = await _mediator.DispatchAsync<
            UploadImageForProductsCommand,
            ResultViewModel<bool>
        >(command);

        return !response.IsSuccess ? BadRequest(response.Message) : Ok(response);
    }

    [HttpGet("{id:guid}/image/{imageId:guid}")]
    public async Task<IActionResult> DownloadImage(Guid id, Guid imageId)
    {
        DownloadImageForProductQuery query = new(imageId);

        ResultViewModel<Stream> result = await _mediator.DispatchAsync<
            DownloadImageForProductQuery,
            ResultViewModel<Stream>
        >(query);

        if (!result.IsSuccess)
            return StatusCode((int)(result.ErrorCode ?? HttpStatusCode.BadRequest), result.Message);

        if (result.Data is null)
            return NotFound("Image not found");

        return File(result.Data!, "image/png");
    }
}
