using CustomerManagement.Core.Requests.Utils;

namespace CustomerManagement.Core.Requests.Customer;

public class CreateCustomerRequest : UserValidatedRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int Type { get; set; }
}
