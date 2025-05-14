using System;
using Arzand.Modules.Payment.Application;
using Arzand.Modules.Payment.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Arzand.Api.Controllers.Payment;

[ApiController]
[Route("api/payment")]
public class PaymentController : ControllerBase
{
    private readonly PaymentService _service;

    public PaymentController(PaymentService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Pay(CreatePaymentRequestDto dto)
    {
        await _service.ProcessPaymentAsync(dto);
        return Ok();
    }
}