using Contracts.Common.Interfaces;
using Customer.API.Entities;
using Customer.API.Persistence;

namespace Customer.API.Repositories.Interfaces
{
    public interface ICustomerRepository : IRepositoryBaseAsync<CatalogCustomer, int, CustomerContext>
    {
        Task<CatalogCustomer?> GetCustomerByUserNameAsync(string username);
        Task CreateCustomerAsync(CatalogCustomer customer);
        Task UpdateCustomerAsync(CatalogCustomer customer);
        Task DeleteCustomerAsync(string username);

    }
}
