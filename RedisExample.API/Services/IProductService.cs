using RedisExample.API.Models;

namespace RedisExample.API.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAsync();
        Task<Product> GetByIdAsync(int Id);
        Task<Product> CreateAsync(Product product);
    }
}
