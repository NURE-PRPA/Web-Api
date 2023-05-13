using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;

public class Listener : AbstractUser
{
    public List<Ban> Bans { get; set; }
    public List<Subscription> Subscriptions { get; set; }
}