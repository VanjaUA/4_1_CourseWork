using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Warehouse.Application.Interfaces;

namespace Warehouse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StockController : ControllerBase
{
    private readonly IStockService _service;

    public StockController(IStockService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetStock([FromQuery] Guid warehouseId)
    {
        return Ok(await _service.GetStockByWarehouseAsync(warehouseId));
    }

    [HttpGet("valuation")]
    public async Task<IActionResult> GetValuation([FromQuery] Guid warehouseId)
    {
        return Ok(await _service.GetValuationReportAsync(warehouseId));
    }
}
