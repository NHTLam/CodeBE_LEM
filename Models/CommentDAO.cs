using CodeBE_LEM.Entities;
using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Models;

public partial class CommentDAO
{
    public long Id { get; set; }

    public long? ClassEventId { get; set; }

    public string? Description { get; set; }

    public long? JobId { get; set; }

    public virtual ClassEventDAO? ClassEvent { get; set; }

    public virtual JobDAO? Job { get; set; }
}
