using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Entities;

public partial class Comment
{
    public long Id { get; set; }

    public long? ClassEventId { get; set; }

    public string? Description { get; set; }

    public long? JobId { get; set; }

    public Job? Job { get; set; }

    public ClassEvent? ClassEvent { get; set; } = null!;
}
