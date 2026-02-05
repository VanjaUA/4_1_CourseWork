namespace Warehouse.Domain.Entities;

public class Notification
{
    public Guid Id { get; set; }
    public Guid? ProductId { get; set; } // Optional link
    public Guid? WarehouseId { get; set; } // Optional link
    
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
