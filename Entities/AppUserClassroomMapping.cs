using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Entities;

public partial class AppUserClassroomMapping
{
    public long Id { get; set; }

    public long ClassroomId { get; set; }

    public long AppUserId { get; set; }

    public long? RoleId { get; set; }

    public AppUser AppUser { get; set; } = null!;

    public Role Role { get; set; } = null!;

    public Classroom Classroom { get; set; } = null!;
}
