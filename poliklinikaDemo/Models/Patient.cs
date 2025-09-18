using System;
using System.Collections.Generic;

namespace poliklinikaDemo.Models;

public class Patient
{
    public int PatinId { get; set; }

    public string PatinFname { get; set; } = null!;

    public string PatinSname { get; set; } = null!;

    public string? PatinLname { get; set; }

    public DateOnly? PatinBirthday { get; set; }

    public virtual ICollection<Appointment> Appoints { get; set; } = new List<Appointment>();
}