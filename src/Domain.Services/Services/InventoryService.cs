using Domain.Models.ValueObjects;
using Domain.Repositories.Interfaces;
using Domain.Services.Interfaces;


namespace Domain.Services.Services
{
    public class InventoryService(ISkuRepository skuRepository) : IInventoryService
    {
        private readonly ISkuRepository _skuRepository = skuRepository;

        public async Task<AllocationResult> AllocateProduct(int productId, int quantityToAllocate)
        {
            var skus = await _skuRepository.GetSkus(productId);
            var allocatedItems = new List<AllocatedItem>();

            foreach (var sku in skus)
            {
                if (!sku.CanBeAllocated)
                    continue;

                var available = sku.Available;

                if (quantityToAllocate > available)
                {
                    quantityToAllocate -= available;
                    sku.Allocate(available);
                    allocatedItems.Add(new AllocatedItem(sku.Id, available));
                }
                else
                {
                    sku.Allocate(quantityToAllocate);
                    quantityToAllocate = 0;
                    allocatedItems.Add(new AllocatedItem(sku.Id, quantityToAllocate));
                    break;
                }
            }

            return new AllocationResult(quantityToAllocate == 0, quantityToAllocate, allocatedItems.AsReadOnly());
        }
    }
}
