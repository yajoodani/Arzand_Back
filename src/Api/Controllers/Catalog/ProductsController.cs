
using Arzand.Modules.Catalog.Application.Commands;
using Arzand.Modules.Catalog.Application.DTOs;
using Arzand.Modules.Catalog.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Arzand.Api.Controllers.Catalog;

[ApiController]
[Route("api/catalog/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetAllAsync()
    {
        var result = await _mediator.Send(new GetAllProductsQuery());
        return Ok(result);
    }

    [HttpGet("{id:guid}", Name = "Catalog.Products.GetByIdAsync")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id));
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateProductCommand command)
    {
        var productId = await _mediator.Send(command);
        return CreatedAtRoute("Catalog.Products.GetByIdAsync", new { id = productId }, null);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateProductCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        // TODO: Update variants too
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteByIdAsync(Guid id)
    {
        await _mediator.Send(new DeleteProductCommand(id));
        return NoContent();
    }

    // TODO Add and remove variants 
}

