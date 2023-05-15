using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;

public class Answer
{
    public string Id { get; set; }
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
    public Question Question { get; set; }
    public void RemoveCycles()
    {
        Question = null;
    }
    public void InitializeEntity()
    {
        Id = Guid.NewGuid().ToString();
    }
}