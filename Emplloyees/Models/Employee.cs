using System;
using System.Collections.Generic;

namespace Emplloyees.Models;

public partial class Employee
{
    public Guid EmployeeId { get; set; }

    public string Designation { get; set; } = null!;

    public string Document { get; set; } = null!;

    public Guid UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
