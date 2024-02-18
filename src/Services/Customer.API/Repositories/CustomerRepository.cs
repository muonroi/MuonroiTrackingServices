using Contracts.Common.Interfaces;
using Customer.API.Entities;
using Customer.API.Persistence;
using Customer.API.Repositories.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ILogger = Serilog.ILogger;

namespace Customer.API.Repositories
{
    public class CustomerRepository : RepositoryBaseAsync<CatalogCustomer, int, CustomerContext>, ICustomerRepository
    {
        private readonly ILogger _logger;
        public CustomerRepository(CustomerContext dbContext, IUnitOfWork<CustomerContext> unitOfWork, ILogger logger) : base(dbContext, unitOfWork)
        {
            _logger = logger;
        }
        public async Task<CatalogCustomer?> GetCustomerByUserNameAsync(string username)
        {
            _logger.Information($"BEGIN: GetCustomerByUserNameAsync --> {username} <-- ");
            var result = await FindByCondition(x => !string.IsNullOrEmpty(x.UserName) && x.UserName.Equals(username))
                .SingleOrDefaultAsync();
            _logger.Information($"END: GetCustomerByUserNameAsync --> {username} <--. Result --> {JsonConvert.SerializeObject(result)} <-- ");
            return result;
        }

        public async Task CreateCustomerAsync(CatalogCustomer customer)
        {
            _logger.Information($"BEGIN: CreateCustomerAsync --> {JsonConvert.SerializeObject(customer)} <-- ");
            var result = await CreateAsync(customer);
            _logger.Information($"END: CreateCustomerAsync --> {JsonConvert.SerializeObject(customer)} <--. Result --> {result} <-- ");
        }

        public async Task UpdateCustomerAsync(CatalogCustomer customer)
        {
            _logger.Information($"BEGIN: UpdateCustomerAsync --> {JsonConvert.SerializeObject(customer)} <-- ");
            await UpdateAsync(customer);
            _logger.Information($"END: UpdateCustomerAsync --> {JsonConvert.SerializeObject(customer)} <--. Result --> true <-- ");
        }
        public async Task DeleteCustomerAsync(string username)
        {
            _logger.Information($"BEGIN: DeleteCustomerAsync --> {username} <-- ");
            var customer = await GetCustomerByUserNameAsync(username);
            if (customer is not null)
            {
                await DeleteAsync(customer);
            }
            _logger.Information($"END: DeleteCustomerAsync --> {username} <--. Result --> true <-- ");
        }
    }
}
