using InvoiceAPI.DTO;
using InvoiceAPI.Interfaces;
using InvoiceAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceAPI.Controllers;

/// <summary>
/// Controller for 'Customer'
/// </summary>
/// <param name="customerService"></param>
[ApiController]
[Route("api/[controller]")]
public class CustomersController(ICustomerService customerService) : ControllerBase {
    [HttpGet]
    public async Task<ActionResult<List<Customer>>> GetAll()
        => Ok(await customerService.GetAllCustomersAsync());

    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetById(int id)
    {
        var customer = await customerService.GetCustomerByIdAsync(id);
        if (customer == null) return NotFound();
        return Ok(customer);
    }

    [HttpPost]
    public async Task<ActionResult<Customer>> CreateAsync(CreateCustomerDto dto)
    {
        var created = await customerService.AddCustomerAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Customer>> UpdateAsync(int id, CreateCustomerDto dto)
    {
        var customer = new Customer
        {
            Id = id,
            Name = dto.Name,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber
        };

        var updated = await customerService.UpdateCustomerAsync(customer);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    [HttpDelete("hard/{id}")]
    public async Task<IActionResult> HardDelete(int id)
    {
        var result = await customerService.HardDeleteCustomerAsync(id);
        if (!result) return BadRequest();
        return NoContent();
    }

    [HttpDelete("soft/{id}")]
    public async Task<IActionResult> SoftDelete(int id)
    {
        var result = await customerService.SoftDeleteCustomerAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}