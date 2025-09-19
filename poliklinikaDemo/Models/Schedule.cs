using System;
using System.Collections.Generic;

namespace poliklinikaDemo.Models;

public partial class Schedule
{
    public int SchedId { get; set; }

    public DateOnly SchedDate { get; set; }

    public TimeOnly SchedTime { get; set; }

    public int? DocId { get; set; }

    public bool SchedIsClosed { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual Doctor? Doc { get; set; }
}
