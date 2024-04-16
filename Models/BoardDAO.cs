using CodeBE_LEM.Entities;
using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Models;

public partial class BoardDAO
{
    public long Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsFavourite { get; set; }

    public string ImageUrl { get; set; } = null!;

    public long? ClassroomId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ClassroomDAO? Classroom { get; set; }

    public virtual ICollection<AppUserBoardMappingDAO> AppUserBoardMappings { get; set; } = new List<AppUserBoardMappingDAO>();

    public virtual ICollection<CardDAO> Cards { get; set; } = new List<CardDAO>();
}
