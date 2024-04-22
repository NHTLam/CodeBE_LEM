using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Entities;

public partial class Attachment
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

    public Question? Question { get; set; }

    public AppUser? Owner { get; set; }
}
