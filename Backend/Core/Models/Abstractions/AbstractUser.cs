using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models;

// public class User<T> where T: class
// {
//     public T Entity { get; set; }
// }

public class AbstractUser
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Gender { get; set; }
    //public DateOnly DOB { get; set; }
    public string Email { get; set; }
    public DateTime Date { get; set; }
    public long GoogleId { get; set; }
    public string Password { get; set; }
    public void InitializeEntity()
    {
        Id = Guid.NewGuid().ToString();
        Date = DateTime.Now;
    }
}