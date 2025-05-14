using System;
using Arzand.Modules.Payment.Domain;
using Arzand.Modules.Payment.DTOs;
using Arzand.Modules.Payment.Infrastructure;
using Arzand.Shared.Contracts.IntegrationEvents;
using DotNetCore.CAP;

namespace Arzand.Modules.Payment.Application;

public class PaymentService
{
    private readonly PaymentDbContext _dbContext;
    private readonly ICapPublisher _capPublisher;

    public PaymentService(PaymentDbContext dbContext, ICapPublisher capPublisher)
    {
        _dbContext = dbContext;
        _capPublisher = capPublisher;
    }

    public async Task ProcessPaymentAsync(CreatePaymentRequestDto dto)
    {
        var payment = new PaymentModel(dto.OrderId, dto.Amount, dto.Method);
        _dbContext.Payments.Add(payment);
        await _dbContext.SaveChangesAsync();
        
        var integrationEvent = new PaymentCompletedIntegrationEvent { OrderId = dto.OrderId };
        _capPublisher.Publish("payment.payment_completed", integrationEvent);
    }
}