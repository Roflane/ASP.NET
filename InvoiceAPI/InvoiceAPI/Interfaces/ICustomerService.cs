using InvoiceAPI.DTO;
using InvoiceAPI.Models;

namespace InvoiceAPI.Interfaces;

public interface ICustomerService {
    Task<Customer> AddCustomerAsync(CreateCustomerDto customer);
    Task<Customer?> UpdateCustomerAsync(Customer customer);
    Task<bool> SoftDeleteCustomerAsync(int customerId); 
    Task<bool> HardDeleteCustomerAsync(int customerId);
    Task<List<Customer>> GetAllCustomersAsync();
    Task<Customer?> GetCustomerByIdAsync(int customerId);
}