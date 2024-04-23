
using CodeBE_LEM.Common;
using CodeBE_LEM.Entities;
using CodeBE_LEM.Common;

namespace CodeBE_LEM.Entities;

public class ClassEvent : IFilterable
{
    public long Id { get; set; }

    public long ClassroomId { get; set; }

    public string Code { get; set; } = null!;

    public bool IsSubmit { get; set; }

    public string Name { get; set; } = null!;

    public bool IsClassWork { get; set; }

    public long AppUserId { get; set; }

    public bool Pinned { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? StartAt { get; set; }

    public DateTime? EndAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Classroom Classroom { get; set; } = null!;

    public AppUser? AppUser { get; set; } = null!;

    public List<Comment>? Comments { get; set; }

    public List<Question>? Questions { get; set; }
}
