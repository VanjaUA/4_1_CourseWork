using Warehouse.Application.DTOs;
using Warehouse.Application.Interfaces;
using Warehouse.Domain.Entities;
using Warehouse.Domain.Interfaces;

namespace Warehouse.Application.Services;

public class ProductService : IProductService
{
    private readonly IRepository<Product> _productRepo;

    public ProductService(IRepository<Product> productRepo)
    {
        _productRepo = productRepo;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _productRepo.GetAllAsync();
        return products.Select(p => new ProductDto
        {
            Id = p.Id,
            Sku = p.Sku,
            Name = p.Name,
            Unit = p.Unit,
            UnitPrice = p.UnitPrice,
            MinStock = p.MinStock
        });
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        var p = await _productRepo.GetByIdAsync(id);
        if (p == null) return null;
        
        return new ProductDto
        {
            Id = p.Id,
            Sku = p.Sku,
            Name = p.Name,
            Unit = p.Unit,
            UnitPrice = p.UnitPrice,
            MinStock = p.MinStock
        };
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var product = new Product
        {
            Sku = dto.Sku,
            Name = dto.Name,
            Unit = dto.Unit,
            UnitPrice = dto.UnitPrice,
            MinStock = dto.MinStock
        };

        await _productRepo.AddAsync(product);

        return new ProductDto
        {
            Id = product.Id,
            Sku = product.Sku,
            Name = product.Name,
            Unit = product.Unit,
            UnitPrice = product.UnitPrice,
            MinStock = product.MinStock
        };
    }

    public async Task UpdateAsync(Guid id, CreateProductDto dto)
    {
        var product = await _productRepo.GetByIdAsync(id);
        if (product == null) throw new Exception("Product not found");

        product.Sku = dto.Sku;
        product.Name = dto.Name;
        product.Unit = dto.Unit;
        product.UnitPrice = dto.UnitPrice;
        product.MinStock = dto.MinStock;

        await _productRepo.UpdateAsync(product);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _productRepo.DeleteAsync(id);
    }
}
