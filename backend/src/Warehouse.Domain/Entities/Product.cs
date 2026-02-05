namespace Warehouse.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Sku { get; set; } = string.Empty; // Stock Keeping Unit
    public string Name { get; set; } = string.Empty;
    public string Unit { get; set; } = "pcs"; // e.g. kg, pcs, liters
    
    public decimal UnitPrice { get; set; } // Current price (Purchase or Selling guidance)
    public int MinStock { get; set; } // Low stock threshold
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
