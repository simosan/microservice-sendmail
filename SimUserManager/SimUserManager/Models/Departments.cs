using System;
using System.Collections.Generic;

namespace SimUserManager.Models;

public partial class Departments
{
    public int DepartmentNo { get; set; }

    public string Department { get; set; } = null!;

    public string Section { get; set; } = null!;

}
