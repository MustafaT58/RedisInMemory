using RedisExample.API.Models;
using RedisExample.Cache;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisExample.API.Repository
{
    public class ProducRepositoryWithCacheDecorator : IProductRepository
    {
        private const string productKey= "productCache";
        private readonly IProductRepository _productRepository;
        private readonly RedisService _redisService;
        private readonly IDatabase _cacheRepository;

        public ProducRepositoryWithCacheDecorator(IProductRepository productRepository, RedisService redisService)
        {
            _productRepository = productRepository;
            _redisService = redisService;
            _cacheRepository = redisService.GetDb(1);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            var newProducts = await _productRepository.CreateAsync(product);

            if (await _cacheRepository.KeyExistsAsync(productKey))
            {
                await _cacheRepository.HashSetAsync(productKey, product.Id, JsonSerializer.Serialize(newProducts));

            }
            return newProducts;

        }

        public async Task<List<Product>> GetAsync()
        {
            if (!await _cacheRepository.KeyExistsAsync(productKey))
            {
                return await LoadCasheDbAsync();
            }
            var products =new List<Product>();

            var cacheProducts = await _cacheRepository.HashGetAllAsync(productKey);
            foreach (var item in cacheProducts) 
            {
                var product=JsonSerializer.Deserialize<Product>(item.Value);

                products.Add(product);
            }

            return products;
        }



        public async Task<Product> GetByIdAsync(int id)
        {
            if (_cacheRepository.KeyExists(productKey))
            {
                var product = await _cacheRepository.HashGetAsync(productKey, id);
                return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
            }
            var products = await LoadCasheDbAsync();

            return products.FirstOrDefault(x => x.Id == id);



        }
        private async Task<List<Product>> LoadCasheDbAsync()
        {
            var products=await _productRepository.GetAsync();
            products.ForEach(p =>
            {
                _cacheRepository.HashSetAsync(productKey, p.Id, JsonSerializer.Serialize(p));
            });

            return products;
        }


    }
}
