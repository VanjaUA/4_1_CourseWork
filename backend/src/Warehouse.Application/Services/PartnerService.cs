using Warehouse.Application.DTOs;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interfaces;

namespace Warehouse.Application.Services;

public class PartnerService : IPartnerService
{
    private readonly IRepository<Partner> _partnerRepo;

    public PartnerService(IRepository<Partner> partnerRepo)
    {
        _partnerRepo = partnerRepo;
    }

    public async Task<IEnumerable<PartnerDto>> GetAllAsync()
    {
        var partners = await _partnerRepo.GetAllAsync();
        return partners.Select(p => new PartnerDto
        {
            Id = p.Id,
            Name = p.Name,
            ContactInfo = p.ContactInfo,
            Type = p.Type
        });
    }

    public async Task<PartnerDto> CreateAsync(CreatePartnerDto dto)
    {
        var partner = new Partner
        {
            Name = dto.Name,
            ContactInfo = dto.ContactInfo,
            Type = dto.Type
        };
        
        await _partnerRepo.AddAsync(partner);
        
        return new PartnerDto
        {
            Id = partner.Id,
            Name = partner.Name,
            ContactInfo = partner.ContactInfo,
            Type = partner.Type
        };
    }
}
