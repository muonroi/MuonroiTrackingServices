using AutoMapper;
using Customer.API.Entities;
using Infrastructure.Mapping;
using Shared.DTOs.Customers;
using Shared.DTOs.Products;

namespace Customer.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CatalogCustomer, CustomerDto>();
            CreateMap<CreateCustomerDto, CatalogCustomer>();
            CreateMap<UpdateCustomerDto, CatalogCustomer>()
                .IgnoreAllNonExisting();
        }

    }
}
