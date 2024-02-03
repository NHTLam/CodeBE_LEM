using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Entities;

public partial class Todo
{
    public long Id { get; set; }

    public string Description { get; set; } = null!;

    public double CompletePercent { get; set; }

    public long? JobId { get; set; }

    public Job? Job { get; set; }
}
