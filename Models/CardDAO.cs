using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Models;

public partial class CardDAO
{
    public long Id { get; set; }

    public long BoardId { get; set; }

    public string Name { get; set; } = null!;

    public int? Order { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Board Board { get; set; } = null!;

    public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
}
