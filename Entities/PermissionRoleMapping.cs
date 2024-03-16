using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Entities;

public partial class PermissionRoleMapping
{
    public long Id { get; set; }

    public long RoleId { get; set; }

    public long PermissionId { get; set; }

    public Permission Permission { get; set; } = null!;

    public Role Role { get; set; } = null!;
}
