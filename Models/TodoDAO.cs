using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Models;

public partial class TodoDAO
{
    public long Id { get; set; }

    public string Description { get; set; } = null!;

    public bool IsDone { get; set; }

    public long? JobId { get; set; }

    public virtual JobDAO? Job { get; set; }
}
