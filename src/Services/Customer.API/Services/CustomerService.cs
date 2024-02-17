using AutoMapper;
using Contracts.Common.BaseResponse;
using Customer.API.Entities;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services.Interfaces;
using Shared.DTOs.Customers;

namespace Customer.API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;
        private readonly IMapper _mapper;
        public CustomerService(ICustomerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IResult> GetCustomerByUsernameAsync(string username)
        {
            BaseResponse<CustomerDto> result;
            var entity = await _repository.GetCustomerByUserNameAsync(username);
            if (entity is null)
            {
                result = new BaseResponse<CustomerDto>()
                {
                    Result = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"{username} not found"
                };
                return Results.NotFound(result);
            }
            var data = _mapper.Map<CustomerDto>(entity);
            result = new BaseResponse<CustomerDto>()
            {
                Result = data,
                StatusCode = StatusCodes.Status200OK,
                Message = null,
            };
            return Results.Ok(result);
        }

        public async Task<IResult> CreateCustomerAsync(CreateCustomerDto customer)
        {
            BaseResponse<IResult> result;
            var entity = await _repository.GetCustomerByUserNameAsync(customer.UserName);
            if (entity is not null)
            {
                result = new BaseResponse<IResult>()
                {
                    Result = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"{customer.UserName} already exists"
                };
                return Results.BadRequest(result);
            }

            var customerEntity = _mapper.Map<CatalogCustomer>(customer);
            await _repository.CreateCustomerAsync(customerEntity);
            await _repository.SaveChangesAsync();
            result = new BaseResponse<IResult>()
            {
                Result = null,
                StatusCode = StatusCodes.Status200OK,
                Message = null
            };
            return Results.Ok(result);
        }

        public async Task<IResult> UpdateCustomerAsync(string username, UpdateCustomerDto customer)
        {
            BaseResponse<IResult> result;
            var entity = await _repository.GetCustomerByUserNameAsync(username);
            if (entity is null)
            {
                result = new BaseResponse<IResult>()
                {
                    Result = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"{username} already is not exists"
                };
                return Results.NotFound(result);
            }

            var customerEntity = _mapper.Map(customer, entity);
            await _repository.UpdateCustomerAsync(customerEntity);
            await _repository.SaveChangesAsync();

            result = new BaseResponse<IResult>()
            {
                Result = null,
                StatusCode = StatusCodes.Status200OK,
                Message = null
            };
            return Results.Ok(result);
        }

        public async Task<IResult> DeleteCustomerAsync(string username)
        {
            BaseResponse<IResult> result;
            var entity = await _repository.GetCustomerByUserNameAsync(username);
            if (entity is null)
            {
                result = new BaseResponse<IResult>()
                {
                    Result = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = $"{username} already is not exists"
                };
                return Results.NotFound(result);
            }
            await _repository.DeleteCustomerAsync(username);
            await _repository.SaveChangesAsync();
            result = new BaseResponse<IResult>()
            {
                Result = null,
                StatusCode = StatusCodes.Status200OK,
                Message = null
            };
            return Results.Ok(result);
        }
    }
}
