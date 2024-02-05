using OrderingSystem.Shared;

namespace OrderingSystem.Client.Services.CustomerService
{
    public interface ICustomerService 
    {
        PagedList<Customer> PagedCustomers { get; set; }

        Task GetCustomers(int page = 1, int pageSize = 10, string searchTerm = "");

        Task<Customer> GetCustomerById(string custCode);

        Task CreateCustomer(Customer customer);

        Task UpdateCustomer(string custCode, Customer customer);

        Task DeleteCustomer(string custCode);

        List<string> GetRecentlyUpdatedCustomerIds();

        void ClearRecentlyUpdatedCustomerIds();
    }
}
