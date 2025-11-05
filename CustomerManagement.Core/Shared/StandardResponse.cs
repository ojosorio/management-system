using System.Net;

namespace CustomerManagement.Core.Shared;

public class StandardResponse
{
    public HttpStatusCode Code { get; set; } = HttpStatusCode.InternalServerError;
    public bool HasErrors { get; set; } = true;
    public List<string> Messages { get; set; } = [];

    public void Setup(HttpStatusCode code, bool hasErrors, List<string>? messages)
    {
        Code = code;
        HasErrors = hasErrors;
        if (messages != null && messages.Count != 0)
        {
            Messages.AddRange(messages);
        }
    }

    public void Setup(HttpStatusCode code, bool hasErrors)
    {
        Code = code;
        HasErrors = hasErrors;
    }
}

public class StandardResponse<T> : StandardResponse
{
    public T? Payload { get; set; }

    public void Setup(HttpStatusCode code, bool hasErrors, List<string>? messages, T? payload)
    {
        Payload = payload;
        Setup(code, hasErrors, messages);
    }
}
