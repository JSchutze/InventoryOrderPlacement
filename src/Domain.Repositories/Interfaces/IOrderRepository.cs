using Domain.Models.Entities;

namespace Domain.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetOrder(int id);
    }
}