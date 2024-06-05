namespace Domain.Models.Entities
{
    public class StockAllocation(int orderId, int productId, int skuId, int quantity)
    {
        public int OrderId { get; private set; } = orderId;
        public int ProductId { get; private set; } = productId;
        public int SkuId { get; private set; } = skuId;
        public int Quantity { get; private set; } = quantity;
    }
}
