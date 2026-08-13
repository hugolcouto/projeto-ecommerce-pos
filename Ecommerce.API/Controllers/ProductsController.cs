using System.IO.Compression;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Ecommerce.Application;
using Ecommerce.Application.Commands.Products.DownloadAllImagesForProduct;
using Ecommerce.Application.Commands.Products.DownloadImageForProduct;
using Ecommerce.Application.Commands.ProductsCommands.Products.CreateProduct;
using Ecommerce.Application.Common;
using Ecommerce.Application.Queries.Products.GetAllProducts;
using Ecommerce.Application.Queries.Products.GetProductDetails;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace Ecommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

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

    [HttpGet("{id:guid}/images")]
    public async Task<IActionResult> DownloadAllImages(Guid id)
    {
        DownloadAllImagesForProductQuery query = new(id);

        ResultViewModel<List<Stream>> results = await _mediator.DispatchAsync<
            DownloadAllImagesForProductQuery,
            ResultViewModel<List<Stream>>
        >(query);

        List<Stream> streams = results.Data ?? [];

        MemoryStream memoryStream = new();

        using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            foreach (var stream in streams)
            {
                ZipArchiveEntry? entry = zipArchive.CreateEntry(
                    $"{Guid.NewGuid().ToString()}.jpeg"
                );

                using Stream? entryStream = entry.Open();

                stream.CopyTo(entryStream);
            }
        }

        memoryStream.Position = 0;

        string zipFileName = $"image_{id}.zip";

        return File(memoryStream, "application/zip", zipFileName);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Details(Guid id)
    {
        GetProductDetailsQuery query = new(id);

        ResultViewModel<ProductDetailsViewModel> response = await _mediator.DispatchAsync<
            GetProductDetailsQuery,
            ResultViewModel<ProductDetailsViewModel>
        >(query);

        if (response.IsSuccess is false)
            return NotFound(response.Message);

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        GetAllProductsQuery query = new();

        ResultViewModel<List<GetAllProductQueryItemViewModel>>? response =
            await _mediator.DispatchAsync<
                GetAllProductsQuery,
                ResultViewModel<List<GetAllProductQueryItemViewModel>>
            >(query);

        if (response.IsSuccess is false)
            return BadRequest(response.Message);

        return Ok(response);
    }
}
