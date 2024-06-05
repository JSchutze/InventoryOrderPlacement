using Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories
{
    public class InventoryOrderDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Sku> Skus { get; set; }

    }
}
