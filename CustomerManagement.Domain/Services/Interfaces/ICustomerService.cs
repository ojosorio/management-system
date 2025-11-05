using CustomerManagement.Core.Requests.Customer;
using CustomerManagement.Core.Responses.Customer;
using CustomerManagement.Models;

namespace CustomerManagement.Services.Interfaces;

public interface ICustomerService
{
    public List<Customer> GetAll();

    public Customer? GetById(int id);

    public void Add(Customer customer);
    public CreateCustomerResponse Add(CreateCustomerRequest customer);

    public void Update(Customer customer);

    public void Delete(int id);

}
