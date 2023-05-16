using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Models;

//[NotMapped]
public class UserAttempt
{
    public string Id { get; set; }
    public byte Mark { get; set; }
    public string SubscriptionId { get; set; }
    public string TestId { get; set; }
    public DateTime TimeStamp { get; set; }
    
    [JsonIgnore] public Subscription? Subscription { get; set; }
    [JsonIgnore] public Test? Test { get; set; }
    public void InitializeEntity()
    {
        Id = Guid.NewGuid().ToString();
        TimeStamp = DateTime.Now;
    }
}