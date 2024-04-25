using CodeBE_LEM.Controllers.AppUserController;
using CodeBE_LEM.Controllers.PermissionController;
using CodeBE_LEM.Entities;
using CodeBE_LEM.Models;
using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Controllers.BoardController;

public partial class Board_AppUserDTO
{
    public long Id { get; set; }

    public string? FullName { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Gender { get; set; }

    public string? Password { get; set; }

    public long? StatusId { get; set; }

    public Board_AppUserDTO() { }
    public Board_AppUserDTO(AppUser user)
    {
        this.Id = user.Id;
        this.FullName = user.FullName;
        this.UserName = user.UserName;
        this.Email = user.Email;
        this.Phone = user.Phone?.Trim();
        this.Gender = user.Gender?.Trim();
        this.Password = user.Password?.Trim();
        this.StatusId = user.StatusId;
    }
}
