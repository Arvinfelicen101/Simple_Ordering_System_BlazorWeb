using Microsoft.AspNetCore.Components;
using OrderingSystem.Client.Pages;
using OrderingSystem.Shared;
using OrderingSystem.Shared.ViewModels;
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

        public async Task CreateOrder(OrderViewModel order)
        {
           
                HttpResponseMessage response = await _http.PostAsJsonAsync("api/order", order);

                
        }
    }
}
