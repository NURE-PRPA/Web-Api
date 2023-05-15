using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;

public class ContentContainer
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public byte[] Binary { get; set; }
    public CourseModule Module { get; set; }
    public void InitializeEntity()
    {
        Id = Guid.NewGuid().ToString();
        Date = DateTime.Now;
    }
}