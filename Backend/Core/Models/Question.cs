using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace Core.Models;

public class Question
{
    public int Id { get; set; }
    public string Text { get; set; }
    public QuestionType Type { get; set; }
    public Test Test { get; set; }
    public List<Answer> Answers { get; set; }
}