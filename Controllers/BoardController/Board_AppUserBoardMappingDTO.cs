using CodeBE_LEM.Entities;
using CodeBE_LEM.Models;
using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Controllers.BoardController;

public partial class Board_AppUserBoardMappingDTO
{
    public long Id { get; set; }

    public long AppUserId { get; set; }

    public long BoardId { get; set; }

    public long AppUserTypeId { get; set; }

    public Board_AppUserDTO? AppUser { get; set; }

    public Board_AppUserBoardMappingDTO() { }

    public Board_AppUserBoardMappingDTO(AppUserBoardMapping AppUserBoardMapping)
    {
        Id = AppUserBoardMapping.Id;
        AppUserId = AppUserBoardMapping.AppUserId;
        BoardId = AppUserBoardMapping.BoardId;
        AppUserTypeId = AppUserBoardMapping.AppUserTypeId;
        AppUser = AppUserBoardMapping.AppUser == null ? null : new Board_AppUserDTO(AppUserBoardMapping.AppUser);
    }
}
