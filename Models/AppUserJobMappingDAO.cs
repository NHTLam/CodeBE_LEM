using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Models;

public partial class AppUserJobMappingDAO
{
    public long Id { get; set; }

    public long AppUserId { get; set; }

    public long JobId { get; set; }

    public virtual AppUserDAO AppUser { get; set; } = null!;

    public virtual JobDAO Job { get; set; } = null!;

}
