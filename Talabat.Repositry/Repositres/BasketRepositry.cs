﻿using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.RepostriesInterfaces;

namespace Talabat.Repositry.Repositres
{
    public class BasketRepositry : IBasketRepositry
    {
        private readonly IDatabase _database;
        public BasketRepositry(IConnectionMultiplexer redis) 
        {
            _database=redis.GetDatabase();

        }
        public async Task<bool> DeleteBasketAsync(string basketId)
        {
          return await  _database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
           var basket= await _database.StringGetAsync(basketId);
            return basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
          var CreatedOrUpdated=await _database.StringSetAsync(basket.Id,JsonSerializer.Serialize(basket),TimeSpan.FromDays(value:30));
            if (CreatedOrUpdated is false) return null;
            return await GetBasketAsync(basket.Id);
        }
    }
}
