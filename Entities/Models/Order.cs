﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Order
{
    public int Id { get; set; }

    public string UserId { get; set; }

    public DateTime OrderDate { get; set; }

    public decimal Total { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual AspNetUser User { get; set; }
}