using Warehouse.Application.DTOs;
using Warehouse.Application.Interfaces;
using Warehouse.Application.Patterns;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interfaces;

namespace Warehouse.Application.Services;

public class StockService : IStockService
{
    private readonly IRepository<StockMovement> _movementRepo;
    private readonly IRepository<Product> _productRepo;
    private readonly IValuationStrategy _valuationStrategy;

    public StockService(
        IRepository<StockMovement> movementRepo, 
        IRepository<Product> productRepo,
        IValuationStrategy valuationStrategy)
    {
        _movementRepo = movementRepo;
        _productRepo = productRepo;
        _valuationStrategy = valuationStrategy;
    }

    public async Task<IEnumerable<StockItemDto>> GetStockByWarehouseAsync(Guid warehouseId)
    {
        var movements = await _movementRepo.FindAsync(m => m.WarehouseId == warehouseId);
        
        // Group by Product
        var grouped = movements
            .GroupBy(m => m.ProductId)
            .Select(g => new { ProductId = g.Key, Qty = g.Sum(x => x.QuantityDelta) })
            .Where(x => x.Qty > 0) // Only show positive stock
            .ToList();

        var result = new List<StockItemDto>();
        
        foreach (var item in grouped)
        {
            var product = await _productRepo.GetByIdAsync(item.ProductId);
            if (product != null)
            {
                result.Add(new StockItemDto
                {
                    ProductId = product.Id,
                    Sku = product.Sku,
                    ProductName = product.Name,
                    Quantity = item.Qty
                });
            }
        }
        
        return result;
    }

    public async Task<ValuationReportDto> GetValuationReportAsync(Guid warehouseId)
    {
        var stockItems = await GetStockByWarehouseAsync(warehouseId);
        var reportItems = new List<ValuationItemDto>();
        decimal totalValue = 0;

        foreach (var item in stockItems)
        {
            var product = await _productRepo.GetByIdAsync(item.ProductId);
            if (product != null)
            {
                var val = _valuationStrategy.CalculateValue(item.Quantity, product.UnitPrice);
                totalValue += val;
                
                reportItems.Add(new ValuationItemDto
                {
                    ProductName = product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = product.UnitPrice,
                    TotalValue = val
                });
            }
        }

        return new ValuationReportDto
        {
            GeneratedAt = DateTime.UtcNow,
            TotalValue = totalValue,
            Items = reportItems
        };
    }
}
