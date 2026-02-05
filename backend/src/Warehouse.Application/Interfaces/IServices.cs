using Warehouse.Application.DTOs;

namespace Warehouse.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task RegisterAsync(RegisterRequest request);
}

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task<ProductDto?> GetByIdAsync(Guid id);
    Task<ProductDto> CreateAsync(CreateProductDto dto);
    Task UpdateAsync(Guid id, CreateProductDto dto);
    Task DeleteAsync(Guid id);
}

public interface IWarehouseService
{
    Task<IEnumerable<WarehouseDto>> GetAllAsync();
    Task<WarehouseDto> CreateAsync(CreateWarehouseDto dto);
    Task DeleteAsync(Guid id);
}

public interface IPartnerService
{
    Task<IEnumerable<PartnerDto>> GetAllAsync();
    Task<PartnerDto> CreateAsync(CreatePartnerDto dto);
}

public interface IDocumentService
{
    Task<IEnumerable<DocumentDto>> GetAllAsync();
    Task<DocumentDto> GetByIdAsync(Guid id);
    Task<DocumentDto> CreateAsync(CreateDocumentDto dto, Guid userId); // Uses Command Logic internally
    Task DeleteAsync(Guid id);
}

public interface IStockService
{
    Task<IEnumerable<StockItemDto>> GetStockByWarehouseAsync(Guid warehouseId);
    Task<ValuationReportDto> GetValuationReportAsync(Guid warehouseId); // Uses Strategy
}

public interface INotificationService
{
    Task<IEnumerable<NotificationDto>> GetUnreadAsync();
    Task MarkAsReadAsync(Guid id);
}

// DTO helper for Notification
public class NotificationDto 
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
