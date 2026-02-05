namespace Warehouse.Application.DTOs;

public class StockItemDto
{
    public Guid ProductId { get; set; }
    public string Sku { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
}

public class ValuationReportDto
{
    public DateTime GeneratedAt { get; set; }
    public decimal TotalValue { get; set; }
    public List<ValuationItemDto> Items { get; set; } = new();
}

public class ValuationItemDto
{
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalValue { get; set; }
}
