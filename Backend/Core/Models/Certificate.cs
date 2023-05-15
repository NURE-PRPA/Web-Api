using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;

public class Certificate
{
    public string Id { get; set; }
    public DateTime Date { get; set; }
    public byte Mark { get; set; }
    public string SubscriptionId { get; set; }
    public Subscription Subscription { get; set; }
    public void InitializeEntity()
    {
        Id = Guid.NewGuid().ToString();
        Date = DateTime.Now;
    }
}