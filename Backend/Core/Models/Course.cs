using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace Core.Models;

public class Course
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public CourseDifficulty Difficulty { get; set; }
    public byte Price { get; set; }
    public CourseTopic Topic { get; set; }
    public DateTime Date { get; set; }
    public Lecturer Lecturer { get; set; }
    public List<CourseModule> Modules { get; set; }
    public List<Subscription> Subscriptions { get; set; }
    public void RemoveCycles()
    {
        if(Lecturer != null)
            Lecturer.Courses = null;

        if(Modules != null)
        {
            foreach (var module in Modules)
                module.Course = null;
        }

        if (Subscriptions != null)
        {
            foreach (var subscription in Subscriptions)
                subscription.Course = null;
        }
    }
    public void InitializeEntity()
    {
        Id = Guid.NewGuid().ToString();
        //Date = DateTime.Now;
    }
}