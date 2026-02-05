namespace Warehouse.Domain.Entities;

public class DocumentItem
{
    public Guid Id { get; set; }
    
    public Guid DocumentId { get; set; }
    public Document? Document { get; set; }
    
    public Guid ProductId { get; set; }
    public Product? Product { get; set; }
    
    public int Quantity { get; set; }
}
