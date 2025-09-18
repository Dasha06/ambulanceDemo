using System.Collections.Generic;

namespace poliklinikaDemo.Models;

public class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<Doctor> Docs { get; set; } = new List<Doctor>();
}