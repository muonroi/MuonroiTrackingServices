using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Contracts.Common.BaseResponse;
using Microsoft.AspNetCore.Mvc;
using Product.API.Entities;
using Product.API.Repositories.Interfaces;
using Shared.DTOs.Products;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _catalogProductRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository catalogProductRepository, IMapper mapper)
        {
            _catalogProductRepository = catalogProductRepository;
            _mapper = mapper;
        }

        #region CRUD

        [HttpGet]
        public async Task<IActionResult> Products()
        {
            BaseResponse<IEnumerable<ProductDto>> result;
            var products = await _catalogProductRepository.GetProductsAsync();
            if (!products.Any())
            {
                result = new BaseResponse<IEnumerable<ProductDto>>()
                {
                    Result = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = nameof(StatusCodes.Status404NotFound)
                };
                return NotFound(result);
            }
            var data = _mapper.Map<IEnumerable<ProductDto>>(products);
            result = new BaseResponse<IEnumerable<ProductDto>>()
            {
                Result = data,
                StatusCode = StatusCodes.Status200OK,
                Message = null
            };

            return Ok(result);
        }
        [HttpGet("{id:long}")]
        public async Task<IActionResult> ProductsById([Required] long id)
        {
            BaseResponse<ProductDto> result;
            var product = await _catalogProductRepository.GetProductById(id);
            if (product is null)
            {
                result = new BaseResponse<ProductDto>()
                {
                    Result = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = nameof(StatusCodes.Status404NotFound)
                };
                return NotFound(result);
            }
            var data = _mapper.Map<ProductDto>(product);
            result = new BaseResponse<ProductDto>()
            {
                Result = data,
                StatusCode = StatusCodes.Status200OK,
                Message = null
            };
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Product([FromBody] CreateProductDto productDto)
        {
            BaseResponse<ProductDto> result;
            var productByNoExist = await _catalogProductRepository.GetProductByNo(productDto.No);
            if (productByNoExist is not null)
            {
                result = new BaseResponse<ProductDto>()
                {
                    Result = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = $"Product No:{productDto.No} is existed"
                };
                return BadRequest(result);
            }
            var product = _mapper.Map<CatalogProduct>(productDto);
            await _catalogProductRepository.CreateProduct(product);
            await _catalogProductRepository.SaveChangesAsync();
            var data = _mapper.Map<ProductDto>(product);
            result = new BaseResponse<ProductDto>()
            {
                Result = data,
                StatusCode = StatusCodes.Status200OK,
                Message = null
            };
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> Product([Required] long id, [FromBody] CreateProductDto productDto)
        {
            BaseResponse<ProductDto> result;
            var product = await _catalogProductRepository.GetProductById(id);
            if (product is null)
            {
                result = new BaseResponse<ProductDto>()
                {
                    Result = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = nameof(StatusCodes.Status404NotFound)
                };
                return NotFound(result);
            }
            var updateProduct = _mapper.Map(productDto, product);
            await _catalogProductRepository.UpdateProduct(updateProduct);
            await _catalogProductRepository.SaveChangesAsync();
            var data = _mapper.Map<ProductDto>(product);
            result = new BaseResponse<ProductDto>()
            {
                Result = data,
                StatusCode = StatusCodes.Status200OK,
                Message = null
            };
            return Ok(result);
        }
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Product([Required] long id)
        {
            BaseResponse<ProductDto> result;
            var product = await _catalogProductRepository.GetProductById(id);
            if (product is null)
            {
                result = new BaseResponse<ProductDto>()
                {
                    Result = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = nameof(StatusCodes.Status404NotFound)
                };
                return NotFound(result);
            }
            await _catalogProductRepository.DeleteProduct(id);
            await _catalogProductRepository.SaveChangesAsync();
            result = new BaseResponse<ProductDto>()
            {
                Result = null,
                StatusCode = StatusCodes.Status200OK,
                Message = null
            };
            return Ok(result);
        }

        #endregion

        #region Additional Resources

        [HttpGet("no/{productNo}")]
        public async Task<IActionResult> ProductsByNo([Required] string productNo)
        {
            BaseResponse<ProductDto> result;
            var product = await _catalogProductRepository.GetProductByNo(productNo);
            if (product is null)
            {
                result = new BaseResponse<ProductDto>()
                {
                    Result = null,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = nameof(StatusCodes.Status404NotFound)
                };
                return NotFound(result);
            }
            var data = _mapper.Map<ProductDto>(product);
            result = new BaseResponse<ProductDto>()
            {
                Result = data,
                StatusCode = StatusCodes.Status200OK,
                Message = null
            };
            return Ok(result);
        }

        #endregion
    }
}
