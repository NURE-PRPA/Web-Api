﻿using System;
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
    public Subscription Subscription { get; set; }
}