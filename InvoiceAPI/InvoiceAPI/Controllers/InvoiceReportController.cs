using InvoiceAPI.DTO;
using InvoiceAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoiceReportController(IInvoiceReportService invoiceReportService) : ControllerBase {
    [HttpPost("customer")]  
    public async Task<ActionResult<List<CustomerReportDto>>> GetCustomerReportAsync([FromBody] DateRangeDto dateRange) {
        var report = await invoiceReportService.GetCustomerReportAsync(dateRange.StartDate, dateRange.EndDate);
        return Ok(report);
    }

    [HttpPost("service")]  
    public async Task<ActionResult<List<ServiceReportDto>>> GetServiceReportAsync([FromBody] DateRangeDto dateRange) 
    {
        var report = await invoiceReportService.GetServiceReportAsync(dateRange.StartDate, dateRange.EndDate);
        return Ok(report);
    }
    
    [HttpPost("status")]  
    public async Task<ActionResult<List<InvoiceReportDto>>> GetInvoiceStatusReportAsync([FromBody] DateRangeDto dateRange) 
    {
        var report = await invoiceReportService.GetInvoiceStatusReportAsync(dateRange.StartDate, dateRange.EndDate);
        return Ok(report);
    }
}