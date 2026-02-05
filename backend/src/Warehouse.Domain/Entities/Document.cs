using Warehouse.Domain.Enums;

namespace Warehouse.Domain.Entities;

public class Document
{
    public Guid Id { get; set; }
    public DocumentType Type { get; set; }
    public string Number { get; set; } = string.Empty; // Invoice/Waybill Number
    
    public Guid WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }
    
    public Guid? PartnerId { get; set; } // Supplier for Receipt, Customer for Shipment
    public Partner? Partner { get; set; }
    
    public Guid CreatedByUserId { get; set; }
    public User? CreatedByUser { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public List<DocumentItem> Items { get; set; } = new();
}
