using InvoiceAPI.DTO;
using InvoiceAPI.Models;

namespace InvoiceAPI.Interfaces;

public interface ICustomerService {
    Task<Customer> CreateCustomerAsync(CreateCustomerDto dto);
    Task<PagedResult<Customer>> GetCustomersAsync(CustomerQueryDto query);
    Task<Customer?> UpdateCustomerAsync(Customer customer);
    Task<bool> SoftDeleteCustomerAsync(int customerId); 
    Task<bool> HardDeleteCustomerAsync(int customerId);
    Task<Customer?> GetCustomerByIdAsync(int customerId);
}