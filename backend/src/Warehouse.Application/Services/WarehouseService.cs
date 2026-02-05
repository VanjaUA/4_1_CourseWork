using Warehouse.Application.DTOs;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.Interfaces;

namespace Warehouse.Application.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IRepository<Warehouse.Domain.Entities.Warehouse> _warehouseRepo;

    public WarehouseService(IRepository<Warehouse.Domain.Entities.Warehouse> warehouseRepo)
    {
        _warehouseRepo = warehouseRepo;
    }

    public async Task<IEnumerable<WarehouseDto>> GetAllAsync()
    {
        var warehouses = await _warehouseRepo.GetAllAsync();
        return warehouses.Select(w => new WarehouseDto
        {
            Id = w.Id,
            Name = w.Name,
            Location = w.Location
        });
    }

    public async Task<WarehouseDto> CreateAsync(CreateWarehouseDto dto)
    {
        var warehouse = new Warehouse.Domain.Entities.Warehouse
        {
            Name = dto.Name,
            Location = dto.Location
        };

        await _warehouseRepo.AddAsync(warehouse);

        return new WarehouseDto
        {
            Id = warehouse.Id,
            Name = warehouse.Name,
            Location = warehouse.Location
        };
    }

    public async Task DeleteAsync(Guid id)
    {
        await _warehouseRepo.DeleteAsync(id);
    }
}
