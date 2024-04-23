using CodeBE_LEM.Controllers.BoardController;
using CodeBE_LEM.Entities;
using CodeBE_LEM.Controllers.ClassroomController;

namespace CodeBE_LEM.Controllers.ClassroomController
{
    public class Classroom_CommentDTO
    {
        public long Id { get; set; }

        public long? ClassEventId { get; set; }

        public long? JobId { get; set; }

        public string Description { get; set; } = null!;

        public long? AppUserId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public Classroom_AppUserDTO? AppUser { get; set; } = null!;

        public Classroom_CommentDTO() { }
        public Classroom_CommentDTO(Comment Comment)
        {
            Id = Comment.Id == null ? 0 : Comment.Id;
            ClassEventId = Comment.ClassEventId;
            Description = Comment.Description;
            CreatedAt = Comment.CreatedAt;
            UpdatedAt = Comment.UpdatedAt;
            AppUserId = Comment.AppUserId;
            AppUser = Comment.AppUser == null ? null : new Classroom_AppUserDTO(Comment.AppUser);
        }

    }
}
