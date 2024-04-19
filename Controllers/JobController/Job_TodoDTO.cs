using CodeBE_LEM.Entities;
using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Controllers.JobController;

public class Job_TodoDTO
{
    public long Id { get; set; }

    public string Description { get; set; } = null!;

    public bool IsDone { get; set; }

    public long? JobId { get; set; }

    public Job_TodoDTO() { }

    public Job_TodoDTO(Todo Todo)
    {
        Id = Todo.Id;
        Description = Todo.Description;
        IsDone = Todo.IsDone;
        JobId = Todo.JobId;
    }
}
