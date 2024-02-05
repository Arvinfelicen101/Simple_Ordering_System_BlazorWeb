using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OrderingSystem.Shared;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace OrderingSystem.Client.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _http;
        private readonly NavigationManager _navigationManager;

        private List<string> recentlyUpdatedProductIds = new List<string>();
        public ProductService(HttpClient http, NavigationManager navigationManager)
        {
            _http = http;
            _navigationManager = navigationManager;
        }
        public PagedList<Product> PagedProducts { get; set; }

        public async Task CreateProduct(Product product)
        {
            try
            {
                HttpResponseMessage response = await _http.PostAsJsonAsync("api/product", product);

                if (response.IsSuccessStatusCode)
                {
                    _navigationManager.NavigateTo("products");
                }
               
            }
            catch (Exception ex)
            {
              
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        public async Task DeleteProduct(string prodCode)
        {
            var result = await _http.DeleteAsync($"api/product/{prodCode}");
        }

        public async Task<Product> GetProductById(string prodCode)
        {
            var result = await _http.GetAsync($"api/product/{prodCode}");
            if(result.StatusCode == HttpStatusCode.OK) {
                return await result.Content.ReadFromJsonAsync<Product>();
            }
            return null;

        }

        public async Task GetProducts(int page = 1, int pageSize = 10, string searchTerm = "")
        {
            try
            {
                

                var result = await _http.GetFromJsonAsync<PagedList<Product>>(
                    $"api/product?page={page}&pageSize={pageSize}&searchTerm={searchTerm}");

                if (result != null)
                {
                    PagedProducts = result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }


        public async Task UpdateProduct(string prodCode, Product product)
{
    try
    {
        ClearRecentlyUpdatedProductIds();

        await _http.PutAsJsonAsync($"api/product/{prodCode}", product);

        recentlyUpdatedProductIds.Add(prodCode);

        _navigationManager.NavigateTo("products");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Exception during UpdateProduct: {ex.Message}");

    }
}

        public List<string> GetRecentlyUpdatedProductIds()
        {
            return recentlyUpdatedProductIds;
        }

        public void ClearRecentlyUpdatedProductIds()
        {
            recentlyUpdatedProductIds.Clear();
        }

    }
}
