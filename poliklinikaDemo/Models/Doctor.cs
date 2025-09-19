using System;
using System.Collections.Generic;

namespace poliklinikaDemo.Models;

public partial class Doctor
{
    public int DocId { get; set; }

    public string DocFname { get; set; } = null!;

    public string DocSname { get; set; } = null!;

    public string? Doclname { get; set; }

    public int? CabId { get; set; }

    public virtual Cabinet? Cab { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
