using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Entities;

public partial class AppUserBoardMapping
{
    public long Id { get; set; }

    public long AppUserId { get; set; }

    public long BoardId { get; set; }

    public long AppUserTypeId { get; set; }

    public AppUser AppUser { get; set; } = null!;

    public Board Board { get; set; } = null!;
}
