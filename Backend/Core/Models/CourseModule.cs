using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Models;

public class CourseModule
{
    public string Id { get; set; }
    public byte Position { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public byte Estimate { get; set; }
    public Test? Test { get; set; }
    public string CourseId { get; set; }
    [JsonIgnore] public Course? Course { get; set; }
    public List<ContentContainer> ContentContainers { get; set; }
    public void RemoveCycles()
    {
        if (Test != null)
            Test.RemoveCycles();

        if (ContentContainers != null)
        {
            foreach (var container in ContentContainers)
                container.Module = null;
        }
    }
    public void InitializeEntity()
    {
        Id = Guid.NewGuid().ToString();
    }
}