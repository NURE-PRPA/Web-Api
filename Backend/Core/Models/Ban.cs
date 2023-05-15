using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;

public class Ban
{
    public string Id { get; set; }
    public string Reason { get; set; }
    public bool IsActive { get; set; }
    public DateTime Date { get; set; }
    public Listener Listener { get; set; }
    public Administrator Administrator { get; set; }
    public void InitializeEntity()
    {
        Id = Guid.NewGuid().ToString();
        Date = DateTime.Now;
    }
}