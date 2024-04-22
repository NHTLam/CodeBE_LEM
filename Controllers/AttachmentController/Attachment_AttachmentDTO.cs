using CodeBE_LEM.Entities;
using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Controllers.AttachmentController
{
    public partial class Attachment_AttachmentDTO
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

        public Attachment_AttachmentDTO() { }

        public Attachment_AttachmentDTO(Attachment Attachment)
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


