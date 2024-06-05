using Domain.Models.Entities;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories.Repositories
{
    public class SkuRepository(InventoryOrderDbContext context) : ISkuRepository
    {
        private readonly InventoryOrderDbContext _context = context;

        public async Task<List<Sku>> GetSkus(int productId)
        {
            return await _context.Skus
                .Where(s => s.ProductId == productId)
                .ToListAsync();
        }

        public async Task<Dictionary<int, Sku>> GetSkus(IEnumerable<int> ids)
        {
            return await _context.Skus
                .Where(s => ids.Contains(s.Id))
                .ToDictionaryAsync(s => s.Id);
        }
    }
}
