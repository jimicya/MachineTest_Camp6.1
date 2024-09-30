using System;
using System.Collections.Generic;

namespace AMSWebApi.Models;

public partial class UserReg
{
    public int UId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public int? Age { get; set; }

    public string? Gender { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public int? LId { get; set; }

    public virtual Login? LIdNavigation { get; set; }
}
