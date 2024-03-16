using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Entities;

public partial class Permission
{
    public long Id { get; set; }

    public string Path { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string MenuName { get; set; } = null!;

    public List<PermissionRoleMapping> PermissionRoleMappings { get; set; } = new List<PermissionRoleMapping>();
}
