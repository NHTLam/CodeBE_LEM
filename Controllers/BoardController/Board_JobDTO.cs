using CodeBE_LEM.Entities;
using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Controllers.BoardController;

public class Board_JobDTO
{
    public long Id { get; set; }

    public long CardId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? Order { get; set; }

    public string? PlanTime { get; set; }

    public string? Color { get; set; }

    public int? NoTodoDone { get; set; }

    public List<Board_TodoDTO> Todos { get; set; } = new List<Board_TodoDTO>();

    public Board_JobDTO() { }

    public Board_JobDTO(Job Job)
    {
        Id = Job.Id;
        CardId = Job.CardId;
        Name = Job.Name;
        Order = Job.Order;
        Description = Job.Description;
        PlanTime = Job.PlanTime;
        Color = Job.Color;
        NoTodoDone = Job.NoTodoDone;
        Todos = Job.Todos?.Select(x => new Board_TodoDTO(x)).ToList();
    }
}
