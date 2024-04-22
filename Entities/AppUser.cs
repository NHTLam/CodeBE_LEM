using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Entities;

public partial class AppUser
{
    public long Id { get; set; }

    public string? FullName { get; set; }

    public string UserName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Gender { get; set; }

    public string Password { get; set; } = null!;

    public long StatusId { get; set; }

    public List<AppUserBoardMapping>? AppUserBoardMappings { get; set; }

    public List<AppUserClassroomMapping>? AppUserClassroomMappings { get; set; }

    public List<AppUserJobMapping>? AppUserJobMappings { get; set; }
}
