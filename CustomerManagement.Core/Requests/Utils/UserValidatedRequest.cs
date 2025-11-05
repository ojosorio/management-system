using CustomerManagement.Core.Requests.Utils.Interfaces;
using System.Text.Json.Serialization;

namespace CustomerManagement.Core.Requests.Utils;

public class UserValidatedRequest : IRequest
{
    [JsonIgnore]
    public string UserName { get; set; } = string.Empty;

    [JsonIgnore]
    public string UserEmail { get; set; } = string.Empty;

    [JsonIgnore]
    public string UserRole { get; set; } = string.Empty;
}
