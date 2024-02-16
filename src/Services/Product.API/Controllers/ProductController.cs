using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
            var products = await _catalogProductRepository.GetProductsAsync();
            if (!products.Any())
            {
                return NotFound();
            }
            var result = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(result);
        }
        [HttpGet("{id:long}")]
        public async Task<IActionResult> ProductsById([Required] long id)
        {
            var product = await _catalogProductRepository.GetProductById(id);
            if (product is null)
            {
                return NotFound();
            }
            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Product([FromBody] CreateProductDto productDto)
        {
            var productByNoExist = await _catalogProductRepository.GetProductByNo(productDto.No);
            if (productByNoExist is not null)
            {
                return BadRequest($"Product No:{productDto.No} is existed ");
            }
            var product = _mapper.Map<CatalogProduct>(productDto);
            await _catalogProductRepository.CreateProduct(product);
            await _catalogProductRepository.SaveChangesAsync();
            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }
        [HttpPut]
        public async Task<IActionResult> Product([Required] long id, [FromBody] CreateProductDto productDto)
        {
            var product = await _catalogProductRepository.GetProductById(id);
            if (product is null)
            {
                return NotFound();
            }
            var updateProduct = _mapper.Map(productDto, product);
            await _catalogProductRepository.UpdateProduct(updateProduct);
            await _catalogProductRepository.SaveChangesAsync();
            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Product([Required] long id)
        {
            var product = await _catalogProductRepository.GetProductById(id);
            if (product is null)
            {
                return NotFound();
            }
            await _catalogProductRepository.DeleteProduct(id);
            await _catalogProductRepository.SaveChangesAsync();
            return Ok();
        }

        #endregion

        #region Additional Resources

        [HttpGet("no/{productNo}")]
        public async Task<IActionResult> ProductsByNo([Required] string productNo)
        {
            var product = await _catalogProductRepository.GetProductByNo(productNo);
            if (product is null)
            {
                return NotFound();
            }
            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }

        #endregion
    }
}
