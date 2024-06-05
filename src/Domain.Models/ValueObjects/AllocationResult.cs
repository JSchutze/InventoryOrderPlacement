using System.Collections.ObjectModel;

namespace Domain.Models.ValueObjects
{
    public record AllocationResult(bool IsFullyAllocated, int NotAllocatedQuantity, ReadOnlyCollection<AllocatedItem> AllocatedItems)
    {
    }
}
