using Warehouse.Domain.Entities;
using Warehouse.Domain.Interfaces;

namespace Warehouse.Application.Patterns;

// Observer Interface
public interface IStockObserver
{
    Task OnStockChangedAsync(Guid productId, Guid warehouseId, int newQuantity, int minStock);
}

// Subject Interface
public interface IStockSubject
{
    void Attach(IStockObserver observer);
    void Detach(IStockObserver observer);
    Task NotifyAsync(Guid productId, Guid warehouseId, int newQuantity, int minStock);
}

// Concrete Observer
public class LowStockObserver : IStockObserver
{
    private readonly IRepository<Notification> _notificationRepo;

    public LowStockObserver(IRepository<Notification> notificationRepo)
    {
        _notificationRepo = notificationRepo;
    }

    public async Task OnStockChangedAsync(Guid productId, Guid warehouseId, int newQuantity, int minStock)
    {
        if (newQuantity < minStock)
        {
            var notification = new Notification
            {
                ProductId = productId,
                WarehouseId = warehouseId,
                Message = $"Warning: Stock for Product {productId} in Warehouse {warehouseId} is low ({newQuantity} < {minStock}).",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            
            await _notificationRepo.AddAsync(notification);
        }
    }
}
