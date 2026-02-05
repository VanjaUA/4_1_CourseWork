using Warehouse.Domain.Entities;

namespace Warehouse.Application.Patterns;

public interface IValuationStrategy
{
    decimal CalculateValue(int quantity, decimal unitPrice);
}

public class CurrentPriceValuationStrategy : IValuationStrategy
{
    public decimal CalculateValue(int quantity, decimal unitPrice)
    {
        return quantity * unitPrice;
    }
}

// Just an example of another strategy, e.g. potentially FIFO/LIFO in a more complex system
// public class FifoValuationStrategy : IValuationStrategy { ... }
