using System;
using System.Collections.Generic;

namespace Emplloyees.Models;

public partial class Role
{
    public Guid RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? UpdatedBy { get; set; }
}
