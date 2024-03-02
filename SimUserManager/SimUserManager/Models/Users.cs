using System;
using System.Collections.Generic;

namespace SimUserManager.Models;

public partial class Users
{

    public string UserId { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int PositionNo { get; set; }

    public int DepartmentNo { get; set; }

}
