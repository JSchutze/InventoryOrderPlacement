using Domain.Models.ValueObjects;

namespace Domain.Services.Interfaces
{
    public interface IInventoryService
    {
        Task<AllocationResult> AllocateProduct(int productId, int quantityToAllocate);
    }
}