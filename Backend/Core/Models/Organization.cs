using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace Core.Models;

public class Organization
{
    public int Id { get; set; }
    public string Name { get; set; }
    public OrganizationType Type { get; set; }
    public List<Lecturer> Lecturers { get; set; }
}