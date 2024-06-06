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

        public async Task AmendOrder(int orderId, List<LineItem> lineItems)
        {
            /* 1. get an order
             * 2. figure out, which line items were changed, we can use IntersectBy for this purpose
             * 3. figure out, what is the delta - negative or positive
             * 4. if positive, it means, more pcs are required, we get SKUs for this position including replacement SKUs and
             * check, if we have enough in stock. If no and CompleteDeliveryRequired, we call CancelOrder() and stop processing
             * 5. if CompleteDeliveryRequired is false, then we try to allocate so much, as we can, to achieve needeed quantity
             * 6. if delta was negative, customer needs less pcs, it is simpler, we just get all allocated skus for the product and
             * deallocate in a loop, while we deallocated this delta
             * 7. we save changes
             */
        }
    }
}
