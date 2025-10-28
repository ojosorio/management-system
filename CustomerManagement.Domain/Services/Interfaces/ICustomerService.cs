using CustomerManagement.Models;

namespace CustomerManagement.Services.Interfaces;

public interface ICustomerService
{
    public List<Customer> GetAll();

    public Customer? GetById(int id);

    public void Add(Customer customer);

    public void Update(Customer customer);

    public void Delete(int id);
}
