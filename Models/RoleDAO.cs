using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Models;

public partial class RoleDAO
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public long? RoleTypeId { get; set; }

    public virtual ICollection<AppUserClassroomMappingDAO> AppUserClassroomMappings { get; set; } = new List<AppUserClassroomMappingDAO>();

    public virtual ICollection<PermissionRoleMappingDAO> PermissionRoleMappings { get; set; } = new List<PermissionRoleMappingDAO>();
}
