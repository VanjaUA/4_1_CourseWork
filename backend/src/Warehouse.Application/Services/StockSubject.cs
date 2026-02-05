using Warehouse.Application.Patterns;

namespace Warehouse.Application.Services;

public class StockSubject : IStockSubject
{
    private readonly List<IStockObserver> _observers = new();

    // In a real DI scenario, we might inject IEnumerable<IStockObserver> to auto-subscribe globally
    public StockSubject(IEnumerable<IStockObserver> observers)
    {
        _observers.AddRange(observers);
    }

    public void Attach(IStockObserver observer)
    {
        _observers.Add(observer);
    }

    public void Detach(IStockObserver observer)
    {
        _observers.Remove(observer);
    }

    public async Task NotifyAsync(Guid productId, Guid warehouseId, int newQuantity, int minStock)
    {
        foreach (var observer in _observers)
        {
            await observer.OnStockChangedAsync(productId, warehouseId, newQuantity, minStock);
        }
    }
}
