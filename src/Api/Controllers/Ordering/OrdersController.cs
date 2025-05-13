using System.Security.Claims;
using Arzand.Modules.Ordering.Application.Commands;
using Arzand.Modules.Ordering.Application.DTOs;
using Arzand.Modules.Ordering.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Arzand.Api.Controllers.Ordering;

[ApiController]
[Route("api/ordering/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetAllForCurrentUserAsync()
    {
        var userId = GetUserId();

        var result = await _mediator.Send(new GetOrdersForUserQuery(userId));
        return Ok(result);
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var userId = GetUserId();

        var result = await _mediator.Send(new GetOrderByIdQuery(userId, id));
        return result is not null ? Ok(result) : NotFound();
    }

    [Authorize]
    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> CancelAsync(Guid id)
    {
        var userId = GetUserId();

        await _mediator.Send(new CancelOrderCommand(userId, id));
        return NoContent();
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrWhiteSpace(userIdClaim))
            throw new Exception("User ID claim not found.");
        return Guid.Parse(userIdClaim);
    }
}
