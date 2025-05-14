using System;

namespace Arzand.Modules.Payment.Domain;

public class PaymentModel
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid OrderId { get; private set; }
    public decimal Amount { get; private set; }
    public string Method { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public PaymentModel(Guid orderId, decimal amount, string method)
    {
        OrderId = orderId;
        Amount = amount;
        Method = method;
    }
}