using System;

namespace Arzand.Shared.Contracts;

public class ReserveStockDto
{
    public Guid ProductId { get; set; }
    public Guid VariantId { get; set; }
    public int Quantity { get; set; }
}
