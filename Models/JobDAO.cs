using CodeBE_LEM.Entities;
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

    public string? Color { get; set; }

    public int? NoTodoDone { get; set; }

    public bool? IsAllDay { get; set; }

    public DateTime? StartAt { get; set; }

    public DateTime? EndAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public long? CreatorId { get; set; }

    public virtual AppUserDAO? Creator { get; set; }

    public virtual ICollection<AppUserJobMappingDAO> AppUserJobMappings { get; set; } = new List<AppUserJobMappingDAO>();

    public virtual CardDAO Card { get; set; } = null!;

    public virtual ICollection<TodoDAO> Todos { get; set; } = new List<TodoDAO>();
}
