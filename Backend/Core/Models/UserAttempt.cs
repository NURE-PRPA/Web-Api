using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;

public class UserAttempt
{
    public int Id { get; set; }
    public byte Mark { get; set; }
    public Subscription Subscription { get; set; }
    public Test Test { get; set; }
}