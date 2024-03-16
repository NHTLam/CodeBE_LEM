using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Models;

public partial class AppUserClassroomMappingDAO
{
    public long Id { get; set; }

    public long ClassroomId { get; set; }

    public long AppUserId { get; set; }

    public virtual AppUserDAO AppUser { get; set; } = null!;

    public virtual ClassroomDAO Classroom { get; set; } = null!;
}
