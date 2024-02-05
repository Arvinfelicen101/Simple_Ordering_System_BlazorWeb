using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using OrderingSystem.Shared;
using System.Net;
using System.Net.Http.Json;

namespace OrderingSystem.Client.Services.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private readonly HttpClient _http;
        private readonly NavigationManager _navigationManager;

        public CustomerService(HttpClient http, NavigationManager navigationManager)
        {
            _http = http;
            _navigationManager = navigationManager;
        }

        private List<string> recentlyUpdatedCustomerIds = new List<string>();
        public PagedList<Customer> PagedCustomers { get; set; }

        public async Task CreateCustomer(Customer customer)
        {
            try
            {
                HttpResponseMessage response = await _http.PostAsJsonAsync("api/customer", customer);

                if (response.IsSuccessStatusCode)
                {
                    _navigationManager.NavigateTo("customers");
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

        public async Task DeleteCustomer(string custCode)
        {
            var result = await _http.DeleteAsync($"api/customer/{custCode}");
        }

        public async Task<Customer> GetCustomerById(string custCode)
        {
            var result = await _http.GetAsync($"api/customer/{custCode}");
            if (result.StatusCode == HttpStatusCode.OK)
            {
                return await result.Content.ReadFromJsonAsync<Customer>();
            }
            return null;

        }

        public async Task GetCustomers(int page = 1, int pageSize = 10, string searchTerm = "")
        {
            try
            {


                var result = await _http.GetFromJsonAsync<PagedList<Customer>>(
                    $"api/customer?page={page}&pageSize={pageSize}&searchTerm={searchTerm}");

                if (result != null)
                {
                    PagedCustomers = result;
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                // Log or handle the exception appropriately
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task UpdateCustomer(string custCode, Customer customer)
        {
            try
            {
                // Clear the previously updated product IDs
                ClearRecentlyUpdatedCustomerIds();

                // Update the product
                await _http.PutAsJsonAsync($"api/customer/{custCode}", customer);

                // Add the newly updated product ID
                recentlyUpdatedCustomerIds.Add(custCode);

                // Delay navigation if needed

                _navigationManager.NavigateTo("customers");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception during UpdateCustomer: {ex.Message}");
                // Handle the exception as needed
            }
        }

        public List<string> GetRecentlyUpdatedCustomerIds()
        {
            return recentlyUpdatedCustomerIds;
        }

        // Add a method to clear recently updated product IDs
        public void ClearRecentlyUpdatedCustomerIds()
        {
            recentlyUpdatedCustomerIds.Clear();
        }
    }
}
