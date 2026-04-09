using Microsoft.EntityFrameworkCore;
using Process360.Core;
using Process360.Core.Models;
using Process360.Repository.Interface;
using Process360.Repository.Repository.Base;

namespace Process360.Repository.Repository;

public class CustomerRepository : RepositoryBase<Customer>, ICustomerRepository
{
    public CustomerRepository(ProcessDbContext context) : base(context)
    {
    }

    public async Task<Customer?> GetCustomerByLoginAsync(string login)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.Login == login);
    }

    public async Task<Customer?> GetCustomerByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<IEnumerable<Customer>> GetActiveCustomersAsync()
    {
        return await _dbSet.Where(c => c.IsActive == true).ToListAsync();
    }
}
