using System;
using System.Collections.Generic;

namespace Emplloyees.Models;

public partial class Profile
{
    public Guid ProfileId { get; set; }

    public Guid? UserId { get; set; }

    public Guid? AddressId { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string Sex { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual Address? Address { get; set; }

    public virtual User? User { get; set; }
}
