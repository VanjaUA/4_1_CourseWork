using Warehouse.Domain.Enums;

namespace Warehouse.Application.DTOs;

public class PartnerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ContactInfo { get; set; } = string.Empty;
    public PartnerType Type { get; set; }
}

public class CreatePartnerDto
{
    public string Name { get; set; } = string.Empty;
    public string ContactInfo { get; set; } = string.Empty;
    public PartnerType Type { get; set; }
}
