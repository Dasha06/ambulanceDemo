using System.Collections.Generic;

namespace poliklinikaDemo.Models;

public class Cabinet
{
    public int CabId { get; set; }

    public string CabNumber { get; set; } = null!;

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}