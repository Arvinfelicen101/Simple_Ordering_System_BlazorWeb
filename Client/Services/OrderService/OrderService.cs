using Microsoft.AspNetCore.Components;
using OrderingSystem.Client.Pages;
using OrderingSystem.Shared;
using System.Net.Http.Json;
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

        public async Task CreateOrder(Order order)
        {
            try
            {
                HttpResponseMessage response = await _http.PostAsJsonAsync("api/order", order);

                if (response.IsSuccessStatusCode)
                {
                    _navigationManager.NavigateTo("orders");
                }
                else
                {
                    // Handle failure
                    // ...
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                // Log or handle the exception appropriately
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }
    }
}
