using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;

public class Subscription
{
    public string Id { get; set; }
    public bool IsActive { get; set; }
    public DateTime Date { get; set; }
    public Listener Listener { get; set; }
    public Certificate Certificate { get; set; }
    public Course Course { get; set; }
    public List<Feedback> Feedbacks { get; set; }
    
    [NotMapped] public List<UserAttempt> UserAttempts { get; set; }
    public void InitializeEntity()
    {
        Id = Guid.NewGuid().ToString();
        Date = DateTime.Now;
    }
}