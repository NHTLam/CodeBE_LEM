using CodeBE_LEM.Entities;
using CodeBE_LEM.Controllers.ClassroomController;

namespace CodeBE_LEM.Controllers.ClassroomController;

public class Classroom_ClassEventDTO
{
    public long Id { get; set; }

    public long ClassroomId { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsClassWork { get; set; }

    public bool Pinned { get; set; }

    public long? AppUserId { get; set; }

    public bool IsSubmit { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? StartAt { get; set; }

    public DateTime? EndAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public List<Classroom_CommentDTO>? Comments { get; set; }

    public Classroom_AppUserDTO? AppUser { get; set; } = null!;

    public List<Classroom_QuestionDTO>? Questions { get; set; }

    public Classroom_ClassEventDTO() { }
    public Classroom_ClassEventDTO(ClassEvent ClassEvent)
    {
        Id = ClassEvent.Id;
        ClassroomId = ClassEvent.ClassroomId;
        Code = ClassEvent.Code;
        Name = ClassEvent.Name;
        Description = ClassEvent.Description;
        Pinned = ClassEvent.Pinned;
        IsClassWork = ClassEvent.IsClassWork;
        CreatedAt = ClassEvent.CreatedAt;
        IsSubmit = ClassEvent.IsSubmit;
        AppUserId = ClassEvent.AppUserId;
        StartAt = ClassEvent.StartAt;
        EndAt = ClassEvent.EndAt;
        UpdatedAt = ClassEvent.UpdatedAt;
        DeletedAt = ClassEvent.DeletedAt;
        AppUser = ClassEvent.AppUser == null ? null : new Classroom_AppUserDTO(ClassEvent.AppUser);
        Comments = ClassEvent.Comments?.Select(x => new Classroom_CommentDTO(x)).ToList();
        Questions = ClassEvent.Questions?.Select(x => new Classroom_QuestionDTO(x)).ToList();
    }
}