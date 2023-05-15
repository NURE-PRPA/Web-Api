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
    public string ModuleId { get; set; }
    public CourseModule Module { get; set; }
    public List<Question> Questions { get; set; }
    
    [NotMapped] public List<UserAttempt> UserAttempts { get; set; }

    public void RemoveCycles()
    {
        if (Module != null)
            Module.Test = null;

        if(Questions != null)
        {
            foreach (var question in Questions)
                question.RemoveCycles();
        }
    }
    public void InitializeEntity()
    {
        Id = Guid.NewGuid().ToString();
        if(Questions != null)
        {
            foreach(var question in Questions)
                question.InitializeEntity();
        }
    }
}