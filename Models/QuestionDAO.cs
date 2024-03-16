using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Models;

public partial class QuestionDAO
{
    public long Id { get; set; }

    public long ClassEventId { get; set; }

    public string Name { get; set; } = null!;

    public string QuestionAnswer { get; set; } = null!;

    public string? StudentAnswer { get; set; }

    public string? Description { get; set; }

    public virtual ClassEventDAO ClassEvent { get; set; } = null!;
}
