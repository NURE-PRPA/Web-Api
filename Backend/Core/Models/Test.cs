using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;

public class Test
{
    public int Id { get; set; }
    public string Name { get; set; }
    public TimeSpan TimeLimit { get; set; }
    public Module Module { get; set; }
    public List<UserAttempt> UserAttempts { get; set; }
    public List<Question> Questions { get; set; }
}