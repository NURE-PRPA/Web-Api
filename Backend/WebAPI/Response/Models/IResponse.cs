namespace WebAPI.Response.Models;

public interface IResponse<T>
{
    public T? Content { get; set; }
    public List<string> Messages { get; set; }
}