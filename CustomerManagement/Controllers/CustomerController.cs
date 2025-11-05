using CustomerManagement.Core.Filters;
using CustomerManagement.Core.Requests.Customer;
using CustomerManagement.Core.Responses.Customer;
using CustomerManagement.Models;
using CustomerManagement.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CustomerManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public ActionResult<List<Customer>> Get() => _customerService.GetAll();

    [HttpGet("{id}")]
    public ActionResult<Customer> Get(int id)
    {
        var customer = _customerService.GetById(id);
        return customer == null ? NotFound() : Ok(customer);
    }

    [HttpPost]
    public IActionResult Create(CreateCustomerRequest customer)
    {
        _customerService.Add(customer);
        return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
    }

    [HttpPost("CreateWithValidation")]
    [SwaggerOperation("CreateWithValidation")]
    [ValidationFilter<CreateCustomerRequest>]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CreateCustomerResponse), StatusCodes.Status400BadRequest)]
    public IActionResult CreateWithValidation([FromBody] CreateCustomerRequest customer)
    {
        var response = _customerService.Add(customer);
        //return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
        return Ok(response);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Customer customer)
    {
        if (id != customer.Id) return BadRequest();
        var existing = _customerService.GetById(id);
        if (existing == null) return NotFound();

        _customerService.Update(customer);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var existing = _customerService.GetById(id);
        if (existing == null) return NotFound();

        _customerService.Delete(id);
        return NoContent();
    }

}
