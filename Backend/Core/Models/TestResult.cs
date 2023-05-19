using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Models;

[NotMapped]
public class TestResult
{
    public string TestId { get; set; }
    public byte Mark { get; set; }
}