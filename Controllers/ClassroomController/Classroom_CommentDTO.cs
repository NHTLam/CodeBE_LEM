using CodeBE_LEM.Entities;

namespace CodeBE_LEM.Controllers.ClassroomController
{
    public class Classroom_CommentDTO
    {
        public long Id { get; set; }

        public long? ClassEventId { get; set; }

        public long? JobId { get; set; }

        public string Description { get; set; } = null!;

        public Classroom_CommentDTO() { }
        public Classroom_CommentDTO(Comment Comment)
        {
            Id = Comment.Id;
            ClassEventId = Comment.ClassEventId;
            Description = Comment.Description;
            JobId = Comment.JobId;
        }

    }
}
