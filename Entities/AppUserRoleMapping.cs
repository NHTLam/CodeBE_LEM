using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Entities;

public partial class AppUserRoleMapping
{
    public long Id { get; set; }

    public long RoleId { get; set; }

    public long AppUserId { get; set; }

    public AppUser AppUser { get; set; } = null!;

    public Role Role { get; set; } = null!;
}
