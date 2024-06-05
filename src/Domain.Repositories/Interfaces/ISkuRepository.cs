using Domain.Models.Entities;

namespace Domain.Repositories.Interfaces
{
    public interface ISkuRepository
    {
        Task<List<Sku>> GetSkus(int productId);
    }
}