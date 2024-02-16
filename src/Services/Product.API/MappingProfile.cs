using AutoMapper;
using Infrastructure.Mapping;
using Product.API.Entities;
using Shared.DTOs.Products;

namespace Product.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CatalogProduct, ProductDto>();
            CreateMap<CreateProductDto, CatalogProduct>();
            CreateMap<UpdateProductDto, CatalogProduct>()
                .IgnoreAllNonExisting();
        }

    }
}
