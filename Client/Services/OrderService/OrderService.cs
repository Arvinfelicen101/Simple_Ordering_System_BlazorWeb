using Microsoft.AspNetCore.Components;
using OrderingSystem.Client.Pages;
using OrderingSystem.Shared;
using OrderingSystem.Shared.ViewModels;
using System.Net.Http.Json;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace OrderingSystem.Client.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly HttpClient _http;
        private readonly NavigationManager _navigationManager;

        public OrderService(HttpClient http, NavigationManager navigationManager)
        {
            _http = http;
            _navigationManager = navigationManager;
        }

        public List<OrderViewModel> PagedOrder { get; set; }

        // public PagedList<OrderViewModel> PagedOrders { get; set; }


        public async Task CreateOrder(IEnumerable<OrderViewModel> OrderViewModel)
        {
            HttpResponseMessage response = await _http.PostAsJsonAsync("api/order", OrderViewModel);
            response.EnsureSuccessStatusCode();
        }


        public async Task AddOrder(OrderViewModel OrderViewModel)
        {
            HttpResponseMessage response = await _http.PostAsJsonAsync("api/order/add-order", OrderViewModel);
            response.EnsureSuccessStatusCode();
        }
        /* public async Task GetOrders(int page = 1, int pageSize = 10, string searchTerm = "")
        {
            try
            {


                var result = await _http.GetFromJsonAsync<PagedList<OrderViewModel>>(
                    $"api/order?page={page}&pageSize={pageSize}&searchTerm={searchTerm}");

                if (result != null)
                {
                    PagedOrders = result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
        */

        public async Task<List<OrderViewModel>> GetOrders()
        {
            var result = await _http.GetFromJsonAsync<List<OrderViewModel>>("api/order");
            if (result != null)
            {
                PagedOrder = result;
                return result; // Add this line to return the result
            }
            else
            {
                return new List<OrderViewModel>(); // Return an empty list if result is null
            }
        }
        public async Task<OrderViewModel> SearchOrder(string searchTerm)
        {
            return await _http.GetFromJsonAsync<OrderViewModel>($"api/order/search?searchTerm={searchTerm}");
        }

        public async Task<List<OrderViewModel>> GetOrdersByOrderNo(string orderNo)
        {
            var orders = await _http.GetFromJsonAsync<List<OrderViewModel>>($"api/order/searchOrder?orderNo={orderNo}");
            return orders;
        }

        public async Task UpdateOrder(string productProdCode, OrderViewModel orderViewModelToUpdate)
        {
            var response = await _http.PutAsJsonAsync($"api/order/{productProdCode}", orderViewModelToUpdate);
            response.EnsureSuccessStatusCode();
        }

        public async Task<OrderViewModel> GetOrderByRowCodeOrderNo(int rowNo, string productProdCode, int orderNo)
        {
            var orderViewModel = await _http.GetFromJsonAsync<OrderViewModel>($"api/order/{rowNo}/{productProdCode}/{orderNo}");
            return orderViewModel;
        }

        public async Task UpdateOrder(int rowNo, string productCode, int orderNo, OrderViewModel updatedOrder)
        {
            
                var orderViewModel = await _http.PutAsJsonAsync($"api/order/{rowNo}/{productCode}/{orderNo}", updatedOrder);
               
          
        }

        public async Task DeleteOrder(int rowNo, string productProdCode, int orderNo)
        {
            var orderViewModel = await _http.DeleteAsync($"api/order/{rowNo}/{productProdCode}/{orderNo}");
        }

            public async Task<int> GetNextRowNo(int orderNo)
            {
                try
                {
                    // Make the HTTP GET request to the server
                    var result = await _http.GetFromJsonAsync<int>($"api/order/nextRowNo/{orderNo}");
                    return result;
                    // Check if the response is successful
                
                }
                catch (Exception ex)
                {
                    // Handle any exceptions
                    Console.WriteLine($"Error: {ex.Message}");
                    throw; // Re-throw the exception
                }
            }


    }
}
