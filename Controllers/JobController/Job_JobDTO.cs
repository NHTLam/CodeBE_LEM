using CodeBE_LEM.Entities;
using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Controllers.JobController;

public class Job_JobDTO
{
    public long Id { get; set; }

    public long? CardId { get; set; }

    public string? Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? Order { get; set; }

    public DateTime? StartAt { get; set; }

    public DateTime? EndAt { get; set; }

    public string? Color { get; set; }

    public int? NoTodoDone { get; set; }

    public bool? IsAllDay { get; set; }

    public long? CreatorId { get; set; }

    public long? ClassroomId { get; set; }

    public Job_AppUserDTO? Creator { get; set; }

    public List<Job_TodoDTO>? Todos { get; set; } = new List<Job_TodoDTO>();

    public List<Job_AppUserJobMappingDTO>? AppUserJobMappings { get; set; } = new List<Job_AppUserJobMappingDTO>();

    public Job_JobDTO() { }

    public Job_JobDTO(Job Job)
    {
        Id = Job.Id;
        CardId = Job.CardId;
        Name = Job.Name;
        Order = Job.Order;
        Description = Job.Description;
        StartAt = Job.StartAt;
        EndAt = Job.EndAt;
        Color = Job.Color;
        NoTodoDone = Job.NoTodoDone;
        IsAllDay = Job.IsAllDay;
        CreatorId = Job.CreatorId;
        Creator = Job.Creator == null ? null : new Job_AppUserDTO(Job.Creator);
        Todos = Job.Todos == null ? null : Job.Todos.Select(x => new Job_TodoDTO(x)).ToList();
        AppUserJobMappings = Job.AppUserJobMappings == null ? null : Job.AppUserJobMappings.Select(x => new Job_AppUserJobMappingDTO(x)).ToList();
    }
}
