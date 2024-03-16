using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Models;

public partial class PermissionRoleMappingDAO
{
    public long Id { get; set; }

    public long RoleId { get; set; }

    public long PermissionId { get; set; }

    public virtual PermissionDAO Permission { get; set; } = null!;

    public virtual RoleDAO Role { get; set; } = null!;
}
