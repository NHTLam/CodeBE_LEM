using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Models;

public partial class JobDAO
{
    public long Id { get; set; }

    public long CardId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? Order { get; set; }

    public string? PlanTime { get; set; }

    public string? Color { get; set; }

    public int? NoTodoDone { get; set; }

    public virtual CardDAO Card { get; set; } = null!;

    public virtual ICollection<TodoDAO> Todos { get; set; } = new List<TodoDAO>();
}
