using CustomerManagement.Infrastructure.Repositories.Interfaces;
using CustomerManagement.Models;
using CustomerManagement.Services.Interfaces;

namespace CustomerManagement.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public List<Customer> GetAll() => _customerRepository.GetAll();

    public Customer? GetById(int id) => _customerRepository.GetById(id);

    public void Add(Customer customer) => _customerRepository.Add(customer);

    public void Update(Customer customer)
    {
        _customerRepository.Update(customer);
    }

    public void Delete(int id)
    {
        _customerRepository.Delete(id);
    }

}
