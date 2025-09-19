using System;
using System.Collections.Generic;

namespace poliklinikaDemo.Models;

public partial class AppointmentsAndPatient
{
    public int PatinId { get; set; }

    public int AppointId { get; set; }

    public virtual Appointment Appoint { get; set; } = null!;

    public virtual Patient Patin { get; set; } = null!;
}
