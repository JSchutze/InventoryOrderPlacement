using Domain.Models.Entities;
using Domain.Services.Interfaces;

namespace Domain.Services.Services
{
    public class OrderService(IInventoryService inventoryService) : IOrderService
    {
        private readonly IInventoryService _inventoryService = inventoryService;

        public async Task PlaceOrders(IEnumerable<Order> orders)
        {
            var sortedOrders = orders.OrderBy(o => o.Priority).ThenBy(x => x.PlacedAt).ToList();

            foreach (var order in sortedOrders)
            {
                await PlaceOrder(order);
            }
        }

        public async Task PlaceOrder(Order order)
        {
            foreach (var item in order.Items)
            {
                var result = await _inventoryService.AllocateProduct(item.ProductId, item.RequiredQuantity);

                if (!result.IsFullyAllocated && order.CompleteDeliveryRequired)
                    return;

                order.AddStockAllocations(result.AllocatedItems);

                if (!result.IsFullyAllocated)
                {
                    // decide, what do to in this case. My suggestion would be to introduce new entity called Batch
                    // to signal, that we will deliver orders in many batches. Or we can create an other order. Etc.
                }
            }

            // save changes
        }
    }
}
