using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Entities;

public partial class Role
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public List<AppUserRoleMapping> AppUserRoleMappings { get; set; } = new List<AppUserRoleMapping>();

    public List<PermissionRoleMapping> PermissionRoleMappings { get; set; } = new List<PermissionRoleMapping>();
}
