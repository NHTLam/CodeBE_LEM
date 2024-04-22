using CodeBE_LEM.Entities;
using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Models;

public partial class AppUserDAO
{
    public long Id { get; set; }

    public string? FullName { get; set; }

    public string UserName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Gender { get; set; }

    public string Password { get; set; } = null!;

    public long StatusId { get; set; }

    public virtual ICollection<AppUserBoardMappingDAO> AppUserBoardMappings { get; set; } = new List<AppUserBoardMappingDAO>();

    public virtual ICollection<AppUserClassroomMappingDAO> AppUserClassroomMappings { get; set; } = new List<AppUserClassroomMappingDAO>();

    public virtual ICollection<AppUserJobMappingDAO> AppUserJobMappings { get; set; } = new List<AppUserJobMappingDAO>();

    public virtual ICollection<JobDAO> Jobs { get; set; } = new List<JobDAO>();
}
