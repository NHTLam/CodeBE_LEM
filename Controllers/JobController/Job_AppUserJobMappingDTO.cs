using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Entities;

public partial class Job_AppUserJobMappingDTO
{
    public long Id { get; set; }

    public long AppUserId { get; set; }

    public long JobId { get; set; }

    public Job_AppUserJobMappingDTO() { }

    public Job_AppUserJobMappingDTO(AppUserJobMapping AppUserJobMapping)
    {
        Id = AppUserJobMapping.Id;
        AppUserId = AppUserJobMapping.AppUserId;
        JobId = AppUserJobMapping.JobId;
    }
}
