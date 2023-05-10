using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;

public class Listener : AbstractUser
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool Gender { get; set; }
    public DateOnly DOB { get; set; }
    public string Email { get; set; }
    public DateTime Date { get; set; }
    public long GoogleId { get; set; }
    public string Password { get; set; }
    public byte BanCount { get; set; }
    public List<Ban> Bans { get; set; }
    public List<Subscription> Subscriptions { get; set; }
}