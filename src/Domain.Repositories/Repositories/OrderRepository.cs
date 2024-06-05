using Domain.Models.Entities;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories.Repositories
{
    public class OrderRepository(InventoryOrderDbContext context) : IOrderRepository
    {
        private readonly InventoryOrderDbContext _context = context;

        public async Task<Order> GetOrder(int id)
        {
            return await _context.Orders
                .Include(x => x.Items)
                .Include(x => x.StockAllocations)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
