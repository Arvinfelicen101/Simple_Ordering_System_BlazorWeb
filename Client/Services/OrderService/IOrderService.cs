using OrderingSystem.Shared.ViewModels;

namespace OrderingSystem.Client.Services.OrderService
{
    public interface IOrderService
    {
        Task CreateOrder(OrderViewModel orderViewModel);
    }
}
