using CodeBE_LEM.Common;
using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Entities;

public partial class Classroom : IFilterable
{
    public long Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? HomeImg { get; set; }

    public List<AppUserClassroomMapping>? AppUserClassroomMappings { get; set; } = new List<AppUserClassroomMapping>();

    public List<ClassEvent>? ClassEvents { get; set; } = new List<ClassEvent>();
}
