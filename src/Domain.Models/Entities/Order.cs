using Domain.Models.Enums;
using Domain.Models.ValueObjects;
using System.Collections.ObjectModel;

namespace Domain.Models.Entities
{
    public class Order
    {
        public int Id { get; private set; }

        private readonly List<LineItem> _items;
        public ReadOnlyCollection<LineItem> Items => _items.AsReadOnly();
        public Priority Priority { get; private set; }
        public bool CompleteDeliveryRequired { get; private set; }
        public DateTime PlacedAt { get; private set; }

        private readonly List<StockAllocation> _stockAllocations;
        public ReadOnlyCollection<StockAllocation> StockAllocations => _stockAllocations.AsReadOnly();

        public void AddStockAllocations(IEnumerable<AllocatedItem> allocatedItems)
        {
            var stockAllocations = allocatedItems.Select(i => new StockAllocation(Id, i.ProductId, i.SkuId, i.AllocatedQuantity));
            _stockAllocations.AddRange(stockAllocations);
        }

        public void Cancel()
        {
            _stockAllocations.Clear();
        }

        public void Cancel(IEnumerable<int> skuIds)
        {
            _stockAllocations.RemoveAll(a => skuIds.Contains(a.SkuId));
        }
    }
}
