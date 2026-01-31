using InvoiceAPI.DTO;
using InvoiceAPI.Enums;
using InvoiceAPI.Interfaces;
using InvoiceAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceAPI.Controllers;

/// <summary>
/// Controller for 'Invoice'
/// </summary>
/// <param name="invoiceService"></param>
[ApiController]
[Route("api/[controller]")]
public class InvoiceController(IInvoiceService invoiceService) : ControllerBase {
    /// <summary>
    /// Asynchronously gets all invoices
    /// </summary>
    /// <returns>List of invoices</returns>
     [HttpGet]
    public async Task<ActionResult<List<Invoice>>> GetAllAsync()
    {
        return Ok(await invoiceService.GetAllInvoicesAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Invoice>> GetById(int id)
    {
        var invoice = await invoiceService.GetInvoiceByIdAsync(id);
        if (invoice == null) return NotFound();
        return Ok(invoice);
    }

    [HttpPost]
    public async Task<ActionResult<Invoice>> CreateAsync(CreateInvoiceDto dto)
    {
        var created = await invoiceService.CreateInvoiceAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Invoice>> UpdateAsync(int id, CreateInvoiceDto dto)
    {
        var invoice = new Invoice
        {
            Id = id,
            CustomerId = dto.CustomerId,
            Comment = dto.Comment,
            Rows = dto.Rows.Select(r => new InvoiceRow
            {
                Service = r.Service,
                Quantity = r.Quantity,
                Rate = r.Rate,
                Sum = r.Quantity * r.Rate
            }).ToList()
        };

        var updated = await invoiceService.UpdateInvoiceAsync(invoice);
        if (updated == null) return BadRequest();
        return Ok(updated);
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> ChangeStatus(int id, [FromBody] EInvoiceStatus status)
    {
        var result = await invoiceService.ChangeInvoiceStatusAsync(id, status);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpDelete("hard/{id}")]
    public async Task<IActionResult> HardDelete(int id)
    {
        var result = await invoiceService.HardDeleteInvoiceAsync(id);
        if (!result) return BadRequest();
        return NoContent();
    }

    [HttpDelete("soft/{id}")]
    public async Task<IActionResult> SoftDelete(int id)
    {
        var result = await invoiceService.SoftDeleteInvoiceAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}