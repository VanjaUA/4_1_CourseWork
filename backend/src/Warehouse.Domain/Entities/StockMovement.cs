using Warehouse.Domain.Enums;

namespace Warehouse.Domain.Entities;

public class StockMovement
{
    public Guid Id { get; set; }
    
    public Guid WarehouseId { get; set; }
    // No nav prop needed strictly if we only use ID for aggregation, but good to have
    // public Warehouse Warehouse {get;set;} 
    
    public Guid ProductId { get; set; }
    // public Product Product {get;set;}
    
    public int QuantityDelta { get; set; } // Positive for Receipt, Negative for Shipment
    
    public Guid DocumentId { get; set; }
    // public Document Document {get;set;}
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
