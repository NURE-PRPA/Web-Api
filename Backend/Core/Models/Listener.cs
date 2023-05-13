using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;

public class Listener : AbstractUser
{
    public int Id { get; set; }
    public List<Ban> Bans { get; set; }
    public List<Subscription> Subscriptions { get; set; }
}