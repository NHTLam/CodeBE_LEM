using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Entities;

public partial class AppUserJobMapping
{
    public long Id { get; set; }

    public long AppUserId { get; set; }

    public long JobId { get; set; }

    public AppUser AppUser { get; set; }

    public Job Job { get; set; }

}
