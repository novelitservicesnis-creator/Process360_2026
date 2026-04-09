using Process360.Core.Models;

namespace Process360.Repository.Interface;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetCustomerByLoginAsync(string login);
    Task<Customer?> GetCustomerByEmailAsync(string email);
    Task<IEnumerable<Customer>> GetActiveCustomersAsync();
}
