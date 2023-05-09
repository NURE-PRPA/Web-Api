using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;

public class Module
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Estimate { get; set; }
    public Test Test { get; set; }
    public Course Course { get; set; }
    public List<ContentContainer> ContentContainers { get; set; }
}