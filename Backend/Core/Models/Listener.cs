namespace Core.Models;

public class Listener : AbstractUser
{
    public int Id { get; set; }
    public List<Ban> Bans { get; set; }
    public List<Subscription> Subscriptions { get; set; }
}