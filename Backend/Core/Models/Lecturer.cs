using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;

public class Lecturer : AbstractUser
{
    public byte Experience { get; set; }
    //public string OrganizationId { get; set; }
    public Organization? Organization { get; set; }
    public List<Course>? Courses { get; set; }

    public Lecturer()
    {
        
    }
}