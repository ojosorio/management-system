using CustomerManagement.Infrastructure.Repositories.Interfaces;
using CustomerManagement.Models;

namespace CustomerManagement.Infrastructure.Repositories;

public class CustomerRepository: ICustomerRepository
{
    private readonly List<Customer> _customer = new();

    public List<Customer> GetAll() => _customer;

    public Customer? GetById(int id) => _customer.FirstOrDefault(p => p.Id == id);

    public void Add(Customer customer) => _customer.Add(customer);

    public void Update(Customer customer)
    {
        var index = _customer.FindIndex(p => p.Id == customer.Id);
        if (index != -1) _customer[index] = customer;
    }

    public void Delete(int id)
    {
        var customer = GetById(id);
        if (customer != null) _customer.Remove(customer);
    }
}
