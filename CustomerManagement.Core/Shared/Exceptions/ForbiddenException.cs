namespace CustomerManagement.Core.Shared.Exceptions;

public class ForbiddenException(string message) : Exception(message)
{
}
