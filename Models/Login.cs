using System;
using System.Collections.Generic;

namespace AMSWebApi.Models;

public partial class Login
{
    public int LId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Usertype { get; set; }

   /* public virtual ICollection<UserReg> UserRegs { get; set; } = new List<UserReg>();*/
}
