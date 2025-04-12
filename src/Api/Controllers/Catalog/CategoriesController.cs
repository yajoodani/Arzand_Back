using Arzand.Modules.Catalog.Application.Commands;
using Arzand.Modules.Catalog.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Arzand.Api.Controllers.Catalog;

[Route("api/catalog/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var result = await _mediator.Send(new GetAllCategoriesQuery());
        return Ok(result);
    }

    [HttpGet("{id:int}", Name = "Catalog.Categories.GetByIdAsync")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        var result = await _mediator.Send(new GetCategoryByIdQuery(id));
        return result is not null ? Ok(result) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateCategoryCommand command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtRoute("Catalog.Categories.GetByIdAsync", new { id }, null);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, UpdateCategoryCommand command)
    {
        if (id != command.Id)
            return BadRequest();

        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        await _mediator.Send(new DeleteCategoryCommand(id));
        return NoContent();
    }
}

