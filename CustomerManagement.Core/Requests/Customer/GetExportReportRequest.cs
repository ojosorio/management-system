using CustomerManagement.Core.Requests.Utils;

namespace CustomerManagement.Core.Requests.Customer;

public class GetExportReportRequest : UserValidatedRequest
{
    public int Id { get; set; }
    public bool All { get; set; }
}
