using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Models;

public partial class ClassroomDAO
{
    public long Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<AppUserClassroomMappingDAO> AppUserClassroomMappings { get; set; } = new List<AppUserClassroomMappingDAO>();

    public virtual ICollection<ClassEventDAO> ClassEvents { get; set; } = new List<ClassEventDAO>();
}
