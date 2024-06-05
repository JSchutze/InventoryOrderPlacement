namespace Domain.Models.Entities
{
    public class StockAllocation(int orderId, int skuId, int quantity)
    {
        public int OrderId { get; private set; } = orderId;
        public int SkuId { get; private set; } = skuId;
        public int Quantity { get; private set;} = quantity;
    }
}
