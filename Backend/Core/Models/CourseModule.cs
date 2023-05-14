using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;

public class CourseModule
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public TimeSpan Estimate { get; set; }
    public Test Test { get; set; }
    public Course Course { get; set; }
    public List<ContentContainer> ContentContainers { get; set; }
    public void RemoveCycles()
    {
        if (Test != null)
            Test.Module = null;
        Course.Modules = null;
        foreach (var container in ContentContainers)
            container.Module = null;
    }
}