using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace Core.Models;

public class Course
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public CourseDifficulty Difficulty { get; set; }
    public byte Price { get; set; }
    public CourseTopic Topic { get; set; }
    public Lecturer Lecturer { get; set; }
    public List<CourseModule> Modules { get; set; }
    public List<Subscription> Subscriptions { get; set; }
}