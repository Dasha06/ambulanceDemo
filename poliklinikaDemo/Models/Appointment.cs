using System.Collections.Generic;

namespace poliklinikaDemo.Models;

public class Appointment
{
    public int AppointId { get; set; }

    public int? SchedId { get; set; }

    public string AppointReason { get; set; } = null!;

    public virtual Schedule? Sched { get; set; }

    public virtual ICollection<Patient> Patins { get; set; } = new List<Patient>();
}