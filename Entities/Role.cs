using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Entities;

public partial class Role
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public long? RoleTypeId { get; set; }

    public List<AppUserClassroomMapping> AppUserClassroomMappings { get; set; } = new List<AppUserClassroomMapping>();

    public List<PermissionRoleMapping> PermissionRoleMappings { get; set; } = new List<PermissionRoleMapping>();
}
