using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Models;

public partial class AppUserBoardMappingDAO
{
    public long Id { get; set; }

    public long AppUserId { get; set; }

    public long BoardId { get; set; }

    public long AppUserTypeId { get; set; }

    public virtual AppUserDAO AppUser { get; set; } = null!;

    public virtual BoardDAO Board { get; set; } = null!;
}
