using Contracts.Common.Interfaces;
using Customer.API.Entities;
using Customer.API.Persistence;
using Customer.API.Repositories.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Repositories
{
    public class CustomerRepository : RepositoryBaseAsync<CatalogCustomer, int, CustomerContext>, ICustomerRepository
    {
        public CustomerRepository(CustomerContext dbContext, IUnitOfWork<CustomerContext> unitOfWork) : base(dbContext, unitOfWork)
        {
        }
        public async Task<CatalogCustomer?> GetCustomerByUserNameAsync(string username)
        {
            return await
            FindByCondition(x => x.UserName.Equals(username))
                .SingleOrDefaultAsync();
        }
        public Task CreateCustomerAsync(CatalogCustomer customer) => CreateAsync(customer);
        public Task UpdateCustomerAsync(CatalogCustomer customer) => UpdateAsync(customer);
        public async Task DeleteCustomerAsync(string username)
        {
            var customer = await GetCustomerByUserNameAsync(username);
            if (customer is not null)
            {
                await DeleteAsync(customer);
            }
        }
    }
}
