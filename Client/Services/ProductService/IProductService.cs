using OrderingSystem.Shared;

namespace OrderingSystem.Client.Services.ProductService
{
    public interface IProductService
    {

        PagedList<Product> PagedProducts { get; set; }

        Task GetProducts(int page = 1, int pageSize = 10, string searchTerm = "");
        Task<Product> GetProductById(string prodCode);

        Task CreateProduct(Product product);

        Task UpdateProduct(string prodCode, Product product);

        Task DeleteProduct(string prodCode);
        List<string> GetRecentlyUpdatedProductIds();

        void ClearRecentlyUpdatedProductIds();
    }
}
