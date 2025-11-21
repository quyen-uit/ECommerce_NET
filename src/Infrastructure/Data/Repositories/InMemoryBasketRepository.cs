using Core.Entities;
using Core.Interfaces.Reposiories;
using System.Collections.Concurrent;

namespace Infrastructure.Data.Repositories
{
    /// <summary>
    /// In-memory implementation of IBasketRepository (replaces Redis-based implementation)
    /// WARNING: This is NOT production-ready. Data is lost on app restart.
    /// </summary>
    public class InMemoryBasketRepository : IBasketRepository
    {
        // Thread-safe dictionary to store baskets
        private static readonly ConcurrentDictionary<string, CustomerBasket> _baskets = new();

        public Task<bool> DeleteBasketAsync(string id)
        {
            var removed = _baskets.TryRemove(id, out _);
            return Task.FromResult(removed);
        }

        public Task<CustomerBasket?> GetBasketAsync(string id)
        {
            _baskets.TryGetValue(id, out var basket);
            return Task.FromResult(basket);
        }

        public Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
        {
            if (basket == null || string.IsNullOrEmpty(basket.Id))
                return Task.FromResult<CustomerBasket?>(null);

            _baskets[basket.Id] = basket;
            return Task.FromResult<CustomerBasket?>(basket);
        }
    }
}
