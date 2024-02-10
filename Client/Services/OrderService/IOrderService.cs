using OrderingSystem.Shared;

namespace OrderingSystem.Client.Services.OrderService
{
    public interface IOrderService
    {
        Task CreateOrder(Order order);
    }
}
