using CodeBE_LEM.Controllers.BoardController;
using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Entities;

public partial class CreateBoardsFunction
{
    public int NumberOfGroups { get; set; }

    public long ClassroomId { get; set; }

    public List<long> AppUserIds { get; set; }
}
