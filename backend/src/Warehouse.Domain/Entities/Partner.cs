using Warehouse.Domain.Enums;

namespace Warehouse.Domain.Entities;

public class Partner
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ContactInfo { get; set; } = string.Empty;
    public PartnerType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
