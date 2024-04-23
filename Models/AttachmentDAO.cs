using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Models;

public partial class AttachmentDAO
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Path { get; set; }

    public string? Capacity { get; set; }

    public long? QuestionId { get; set; }

    public long? OwnerId { get; set; }

    public string? PublicId { get; set; }

    public string? Link { get; set; }

    public virtual AppUserDAO? Owner { get; set; }

    public virtual QuestionDAO? Question { get; set; }
}
