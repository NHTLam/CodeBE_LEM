using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Models;

public partial class CommentDAO
{
    public long Id { get; set; }

    public long ClassEventId { get; set; }

    public string? Description { get; set; }

    public virtual ClassEventDAO ClassEvent { get; set; } = null!;
}
