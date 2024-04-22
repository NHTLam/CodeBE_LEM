using CodeBE_LEM.Entities;
using CodeBE_LEM.Models;
using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Controllers.BoardController;

public partial class Board_CreateBoardsFunctionDTO
{
    public int NumberOfGroups { get; set; }

    public long ClassroomId { get; set; }

    public List<long> AppUserIds { get; set; }

    public Board_CreateBoardsFunctionDTO() { }

    public Board_CreateBoardsFunctionDTO(CreateBoardsFunction CreateBoardsFunction)
    {
        this.NumberOfGroups = CreateBoardsFunction.NumberOfGroups;
        this.ClassroomId = CreateBoardsFunction.ClassroomId;
        this.AppUserIds = CreateBoardsFunction.AppUserIds == null ? null : CreateBoardsFunction.AppUserIds;
    }
}
