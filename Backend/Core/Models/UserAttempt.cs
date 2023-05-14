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
    public int SubscriptionId { get; set; }
    public int TestId { get; set; }
    public DateTime TimeStamp { get; set; }
    
    [JsonIgnore] public Subscription? Subscription { get; set; }
    [JsonIgnore] public Test? Test { get; set; }
}