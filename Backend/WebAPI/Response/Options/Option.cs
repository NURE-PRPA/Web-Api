namespace WebAPI.Response.Options;

public class Option<T>
{
    public T Payload { get; set; }
    public string Message { get; set; }
}