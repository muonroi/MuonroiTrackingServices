using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Product.API.Entities;
using Product.API.Persistence;
using Product.API.Repositories.Interfaces;
using ILogger = Serilog.ILogger;
namespace Product.API.Repositories
{
    public class ProductRepository : RepositoryBaseAsync<CatalogProduct, long, ProductContext>, IProductRepository
    {
        private readonly ILogger _logger;
        public ProductRepository(ProductContext context, IUnitOfWork<ProductContext> unitOfWork, ILogger logger) : base(context, unitOfWork)
        {
            _logger = logger;
        }

        public async Task<IEnumerable<CatalogProduct>> GetProductsAsync()
        {
            _logger.Information($"BEGIN: GetProductsAsync --> ALL <-- ");
            var result = await FindAll().ToListAsync();
            _logger.Information($"END: GetBasketByUsername --> ALL <--. Result --> {JsonConvert.SerializeObject(result)} <-- ");
            return result;
        }

        public async Task<CatalogProduct?> GetProductById(long id)
        {
            _logger.Information($"BEGIN: GetProductById --> {id} <-- ");
            var result = await GetByIdAsync(id);
            _logger.Information($"END: GetProductById --> {id} <--. Result --> {JsonConvert.SerializeObject(result)} <-- ");
            return result;
        }

        public async Task<CatalogProduct?> GetProductByNo(string productNo)
        {
            _logger.Information($"BEGIN: GetProductByNo --> {productNo} <-- ");
            var result = await FindByCondition(x => x.No.Equals(productNo)).SingleOrDefaultAsync();
            _logger.Information($"END: GetProductByNo --> {productNo} <--. Result --> {JsonConvert.SerializeObject(result)} <-- ");
            return result;
        }


        public async Task<long> CreateProduct(CatalogProduct product)
        {
            _logger.Information($"BEGIN: CreateProduct --> {JsonConvert.SerializeObject(product)} <-- ");
            var result = await CreateAsync(product);
            _logger.Information($"END: CreateProduct --> {JsonConvert.SerializeObject(product)} <--. Result --> {result} <-- ");
            return result;
        }

        public Task UpdateProduct(CatalogProduct product)
        {
            _logger.Information($"BEGIN: UpdateProduct --> {JsonConvert.SerializeObject(product)} <-- ");
            UpdateAsync(product);
            _logger.Information($"END: UpdateProduct --> {JsonConvert.SerializeObject(product)} <--. Result true ");
            return Task.CompletedTask;
        }
        public async Task DeleteProduct(long id)
        {
            _logger.Information($"BEGIN: DeleteProduct --> {id} <-- ");
            var product = await GetProductById(id);
            if (product != null)
            {
                await DeleteAsync(product);
            }
            _logger.Information($"END: DeleteProduct --> {id} <--. Result true ");
        }
    }
}
