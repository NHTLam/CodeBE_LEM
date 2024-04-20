using CodeBE_LEM.Models;
using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Entities;

public partial class Job
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

    public AppUser? Creator { get; set; }

    public List<AppUserJobMapping> AppUserJobMappings { get; set; } = new List<AppUserJobMapping>();

    public Card Card { get; set; } = null!;

    public List<Todo> Todos { get; set; } = new List<Todo>();
}
