using Domain.Models.Entities;

namespace Domain.Services.Interfaces
{
    public interface IOrderService
    {
        Task CancelOrder(int orderId);
        Task CancelOrder(int orderId, int[] lineItemIds);
        Task PlaceOrder(Order order);
        Task PlaceOrders(IEnumerable<Order> orders);
    }
}