using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Models;

public partial class PermissionDAO
{
    public long Id { get; set; }

    public string Path { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string MenuName { get; set; } = null!;

    public virtual ICollection<PermissionRoleMappingDAO> PermissionRoleMappings { get; set; } = new List<PermissionRoleMappingDAO>();
}
