using CodeBE_LEM.Entities;

namespace CodeBE_LEM.Controllers.ClassroomController
{
    public class Classroom_AttachmentDTO
    {
        public long Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Path { get; set; }

        public string? Capacity { get; set; }

        public long? QuestionId { get; set; }

        public long? OwnerId { get; set; }

        public string? Link { get; set; }

        public string? PublicId { get; set; }

        public long ClassroomId { get; set; }

        public Classroom_AttachmentDTO() { }
        public Classroom_AttachmentDTO(Attachment Attachment)
        {
            this.Id = Attachment.Id;
            this.Name = Attachment.Name;
            this.Description = Attachment.Description;
            this.Path = Attachment.Path;
            this.Capacity = Attachment.Capacity;
            this.QuestionId = Attachment.QuestionId;
            this.OwnerId = Attachment.OwnerId;
            this.PublicId = Attachment.PublicId;
            this.Link = Attachment.Link;
        }
    }
}
