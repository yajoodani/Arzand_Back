using Arzand.Shared.Application;

namespace Arzand.Modules.Ordering.Application.DTOs;

public record OrderItemDto (
    Guid ProductId,
    Guid VariantId,
    string ProductName,
    MoneyDto UnitPrice,
    int Quantity,
    MoneyDto TotalPrice
);
