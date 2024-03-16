using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Models;

public partial class AppUserRoleMappingDAO
{
    public long Id { get; set; }

    public long RoleId { get; set; }

    public long AppUserId { get; set; }

    public virtual AppUserDAO AppUser { get; set; } = null!;

    public virtual RoleDAO Role { get; set; } = null!;
}
