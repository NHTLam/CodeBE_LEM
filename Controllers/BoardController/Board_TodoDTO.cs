using CodeBE_LEM.Entities;
using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Controllers.BoardController;

public class Board_TodoDTO
{
    public long Id { get; set; }

    public string Description { get; set; } = null!;

    public double CompletePercent { get; set; }

    public long? JobId { get; set; }

    public Board_TodoDTO() { }

    public Board_TodoDTO(Todo Todo)
    {
        Id = Todo.Id;
        Description = Todo.Description;
        CompletePercent = Todo.CompletePercent;
        JobId = Todo.JobId;
    }
}
