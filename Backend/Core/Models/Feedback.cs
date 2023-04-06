﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models;

public class Feedback
{
    public int Id { get; set; }
    public string Comment { get; set; }
    public byte Stars { get; set; }
    public DateTime Date { get; set; }
    public Subscription Subscription { get; set; }
}