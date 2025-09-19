using System;
using System.Collections.Generic;

namespace poliklinikaDemo.Models;

public partial class Appointment
{
    public int AppointId { get; set; }

    public int? SchedId { get; set; }

    public string AppointReason { get; set; } = null!;

    public virtual AppointmentsAndPatient? AppointmentsAndPatient { get; set; }

    public virtual Schedule? Sched { get; set; }
}
