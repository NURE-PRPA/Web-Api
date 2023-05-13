namespace WebAPI.Response.Models;

public class Response<T> : IResponse<T>
{
    public OperationResult Status { get; set; }
    public T? Content { get; set; }
    public List<string> Messages { get; set; } = new();

    public Response(OperationResult status, params string[] messages)
    {
        Messages = new();
        Status = status;
        
        foreach (var message in messages)
        {
            Messages.Add(message);
        }
    }
    
    public Response(OperationResult status, T @object, params string[] messages)
    {
        Messages = new();
        Status = status;
        Content = @object;
        
        foreach (var message in messages)
        {
            Messages.Add(message);
        }
    }

    public Response() { }
}