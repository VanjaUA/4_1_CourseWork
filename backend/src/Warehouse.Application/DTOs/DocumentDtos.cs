using Warehouse.Domain.Enums;

namespace Warehouse.Application.DTOs;

public class DocumentDto
{
    public Guid Id { get; set; }
    public DocumentType Type { get; set; }
    public string Number { get; set; } = string.Empty;
    public string WarehouseName { get; set; } = string.Empty;
    public string? PartnerName { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<DocumentItemDto> Items { get; set; } = new();
}

public class DocumentItemDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

public class CreateDocumentDto
{
    public DocumentType Type { get; set; }
    public Guid WarehouseId { get; set; }
    public Guid? PartnerId { get; set; } // Required for Receipt/Shipment
    public List<CreateDocumentItemDto> Items { get; set; } = new();
}

public class CreateDocumentItemDto
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
