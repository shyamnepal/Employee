using System;
using System.Collections.Generic;

namespace Emplloyees.Models;

public partial class Address
{
    public Guid AddressId { get; set; }

    public string? TemporaryAddress { get; set; }

    public string ParmanentAddress { get; set; } = null!;
    public Guid? UserId { get; set; }
    public string City { get; set; } = null!;

    public string Country { get; set; } = null!;

    public int PostalCode { get; set; }

    public string State { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual ICollection<Profile> Profiles { get; } = new List<Profile>();
}
