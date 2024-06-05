using Domain.Models.Entities;

namespace Domain.Services.Interfaces
{
    public interface IOrderService
    {
        Task PlaceOrder(Order order);
        Task PlaceOrders(IEnumerable<Order> orders);
    }
}