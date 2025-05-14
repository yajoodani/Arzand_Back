using System;

namespace Arzand.Modules.Payment.DTOs;

public class CreatePaymentRequestDto
{
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
    public string Method { get; set; } = default!;
}
