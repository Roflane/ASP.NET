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
    /// <summary>
    /// Asynchronously gets all customers
    /// </summary>
    /// <returns>Paged result of customer</returns>
    [HttpGet]
    public async Task<ActionResult<PagedResult<Customer>>> GetAll(
        [FromQuery] CustomerQueryDto query) 
        => Ok(await customerService.GetCustomersAsync(query));

    /// <summary>
    /// Asynchronously gets customer by id
    /// </summary>
    /// <returns>Action result of customer</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Customer>> GetById(int id) {
        var customer = await customerService.GetCustomerByIdAsync(id);
        if (customer == null) return NotFound();
        return Ok(customer);
    }

    /// <summary>
    /// Asynchronously creates customer by id
    /// </summary>
    /// <param name="dto">CreateCustomerDto object</param>
    /// <returns>Action result of customer</returns>
    [HttpPost]
    public async Task<ActionResult<Customer>> CreateAsync(CreateCustomerDto dto) {
        var created = await customerService.CreateCustomerAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Asynchronously updates the customer by CreateCustomerDto object and id
    /// </summary>
    /// <param name="id">Customer id</param>
    /// <param name="dto">CreateCustomerDto object</param>
    /// <returns>Action result of customer</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<Customer>> UpdateAsync(int id, CreateCustomerDto dto) {
        var customer = new Customer {
            Id = id,
            Name = dto.Name,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber
        };

        var updated = await customerService.UpdateCustomerAsync(customer);
        if (updated == null) return NotFound();
        return Ok(updated);
    }

    /// <summary>
    /// Asynchronously does hard delete of customer
    /// </summary>
    /// <param name="id">Customer id</param>
    /// <returns>Action result of customer</returns>
    [HttpDelete("hard/{id}")]
    public async Task<IActionResult> HardDelete(int id) {
        var result = await customerService.HardDeleteCustomerAsync(id);
        if (!result) return BadRequest();
        return NoContent();
    }

    /// <summary>
    /// Asynchronously does soft delete of customer
    /// </summary>
    /// <param name="id">Customer id</param>
    /// <returns>Action result of customer</returns>
    [HttpDelete("soft/{id}")]
    public async Task<IActionResult> SoftDelete(int id) {
        var result = await customerService.SoftDeleteCustomerAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}