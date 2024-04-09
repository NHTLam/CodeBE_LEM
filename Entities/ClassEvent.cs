using CodeBE_LEM.Common;
using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Entities;

public partial class ClassEvent : IFilterable
{
    public long Id { get; set; }

    public long ClassroomId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool Pinned { get; set; }

    public string? Instruction { get; set; }

    public DateTime? EndAt { get; set; }

    public bool IsClassWork { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public string? Description { get; set; }

    public Classroom Classroom { get; set; } = null!;

    public List<Comment>? Comments { get; set; } = new List<Comment>();

    public List<Question>? Questions { get; set; } = new List<Question>();
}
