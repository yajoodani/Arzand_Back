using System;

namespace Arzand.Shared.Contracts;

public interface ICatalogService
{
    Task<List<ProductVariantPriceDto>> HandleReserveStockAsync(List<ReserveStockDto> reserveStockDtos, CancellationToken cancellationToken = default);
}
