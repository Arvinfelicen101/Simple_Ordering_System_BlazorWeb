using OrderingSystem.Shared;
using OrderingSystem.Shared.ViewModels;

namespace OrderingSystem.Client.Services.OrderService
{
    public interface IOrderService
    {
        // PagedList<OrderViewModel> PagedOrders { get; set; }

        //Task GetOrders(int page = 1, int pageSize = 10, string searchTerm = "");

        List<OrderViewModel> PagedOrder { get; set; }

        Task<List<OrderViewModel>> GetOrders();
        Task CreateOrder(IEnumerable<OrderViewModel> OrderViewModel, int pOrderNo);

        Task AddOrder(OrderViewModel orderViewModel);
        Task<OrderViewModel> SearchOrder(string searchTerm);
        Task<List<OrderViewModel>> GetOrdersByOrderNo(string orderNo);
        Task<List<OrderViewModel>> GetOrdersByOrderNum(int orderNo);

        Task UpdateOrder(string productProdCode, OrderViewModel orderViewModelToUpdate);
        Task<OrderViewModel> GetOrderByRowCodeOrderNo(int rowNo, string productProdCode, int orderNo);

        Task UpdateOrder(int rowNo, string productProdCode, int orderNo, OrderViewModel updatedOrder);

        Task DeleteOrder(int rowNo, string productProdCode, int orderNo);

        Task<int> GetNextRowNo(int orderNo);

        Task<int> GetLatestOrderNumber();
    }
}
