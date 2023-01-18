﻿using RedisExample.API.Models;

namespace RedisExample.API.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAsync();
        Task<Product> GetByIdAsync(int Id);
        Task<Product> CreateAsync(Product product);

    }
}
