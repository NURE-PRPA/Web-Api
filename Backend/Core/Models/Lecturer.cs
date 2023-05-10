using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;

public class Lecturer : AbstractUser
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool Gender { get; set; }
    public DateOnly DOB { get; set; }
    public string Email { get; set; }
    public DateTime Date { get; set; }
    public long GoogleId { get; set; }
    public string Password { get; set; }
    public byte Experience { get; set; }
    public Organization Organization { get; set; }
    public List<Course> Courses { get; set; }
}