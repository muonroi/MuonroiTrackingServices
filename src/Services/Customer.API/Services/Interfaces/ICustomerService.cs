using Contracts.Common.BaseResponse;
using Shared.DTOs.Customers;

namespace Customer.API.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<IResult> GetCustomerByUsernameAsync(string username);
        Task<IResult> CreateCustomerAsync(CreateCustomerDto customer);
        Task<IResult> UpdateCustomerAsync(string username, UpdateCustomerDto customer);
        Task<IResult> DeleteCustomerAsync(string username);
    }
}
