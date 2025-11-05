using AutoMapper;
using CustomerManagement.Core.Requests.Customer;
using CustomerManagement.Core.Responses.Customer;
using CustomerManagement.Core.Shared.Helpers;
using CustomerManagement.Infrastructure.Repositories.Interfaces;
using CustomerManagement.Models;
using CustomerManagement.Services.Interfaces;
using System;

namespace CustomerManagement.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    public List<Customer> GetAll() => _customerRepository.GetAll();

    public Customer? GetById(int id) => _customerRepository.GetById(id);

    public void Add(Customer customer) => _customerRepository.Add(customer);

    public CreateCustomerResponse Add(CreateCustomerRequest customerRequest)
    {
        CreateCustomerResponse createCustomerResponse = new();
        var customer = _mapper.Map<Customer>(customerRequest);
        _customerRepository.Add(customer);

        createCustomerResponse.SetCreatedResponse(customer.Id, ["Customer created"]);
        return createCustomerResponse;
    }

    public void Update(Customer customer)
    {
        _customerRepository.Update(customer);
    }

    public void Delete(int id)
    {
        _customerRepository.Delete(id);
    }

}
