using Domain.Models.Entities;
using Domain.Repositories.Interfaces;
using Domain.Services.Interfaces;

namespace Domain.Services.Services
{
    public class OrderService(IInventoryService inventoryService, IOrderRepository orderRepository,
        ISkuRepository skuRepository) : IOrderService
    {
        private readonly IInventoryService _inventoryService = inventoryService;
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly ISkuRepository _skuRepository = skuRepository;


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

        public async Task CancelOrder(int orderId)
        {
            var order = await _orderRepository.GetOrder(orderId)
                ?? throw new Exception("Order not found");

            var allocations = order.StockAllocations;

            var skus = await _skuRepository.GetSkus(allocations.Select(a => a.SkuId));

            foreach (var allocation in allocations)
            {
                skus[allocation.SkuId].Deallocate(allocation.Quantity);
            }

            order.Cancel();

            // save changes
        }

        public async Task CancelOrder(int orderId, int[] lineItemIds)
        {
            var order = await _orderRepository.GetOrder(orderId)
               ?? throw new Exception("Order not found");

            var productIds = order.Items
                .Where(x => lineItemIds.Contains(x.Id))
                .Select(x => x.ProductId)
                .ToList();

            var allocations = order.StockAllocations.Where(x => productIds.Contains(x.ProductId)).ToList();

            var skus = await _skuRepository.GetSkus(allocations.Select(a => a.SkuId));

            foreach (var allocation in allocations)
            {
                skus[allocation.SkuId].Deallocate(allocation.Quantity);
            }

            order.Cancel(skus.Keys);

            // save changes
        }
    }
}
