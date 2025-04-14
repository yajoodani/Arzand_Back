using Arzand.Modules.Catalog.Application.Commands;
using Arzand.Modules.Catalog.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Arzand.Api.Controllers.Catalog;

[Route("api/catalog/[controller]")]
[ApiController]
public class BrandsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BrandsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _mediator.Send(new GetAllBrandsQuery());
        return Ok(result);
    }

    [HttpGet("{id:guid}", Name = "Catalog.Brands.GetByIdAsync")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var result = await _mediator.Send(new GetBrandByIdQuery(id));
        return result is not null ? Ok(result) : NotFound();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateBrandCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtRoute("Catalog.Brands.GetByIdAsync", new { id }, null);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateBrandCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        await _mediator.Send(command);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        await _mediator.Send(new DeleteBrandCommand(id));
        return NoContent();
    }
}

