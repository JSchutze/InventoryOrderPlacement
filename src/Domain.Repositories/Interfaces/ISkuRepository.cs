using Domain.Models.Entities;

namespace Domain.Repositories.Interfaces
{
    public interface ISkuRepository
    {
        Task<List<Sku>> GetSkus(int productId);
        Task<Dictionary<int, Sku>> GetSkus(IEnumerable<int> ids);
    }
}