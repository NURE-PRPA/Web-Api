using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;

public class Test
{
    public string Id { get; set; }
    public string Name { get; set; }
    public byte Duration { get; set; }
    public CourseModule Module { get; set; }
    public List<Question> Questions { get; set; }
    
    [NotMapped] public List<UserAttempt> UserAttempts { get; set; }
}