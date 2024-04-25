using CodeBE_LEM.Controllers.AppUserController;
using CodeBE_LEM.Controllers.PermissionController;
using CodeBE_LEM.Entities;
using CodeBE_LEM.Models;
using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Controllers.AppUserController;

public partial class AppUser_AppUserDTO
{
    public long Id { get; set; }

    public string? FullName { get; set; }

    public string? UserName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Gender { get; set; }

    public string? Password { get; set; }

    public long? StatusId { get; set; }

    public List<AppUser_AppUserClassroomMappingDTO>? AppUserClassroomMappings { get; set; }

    public AppUser_AppUserDTO() { }
    public AppUser_AppUserDTO(AppUser user)
    {
        this.Id = user.Id;
        this.FullName = user.FullName;
        this.UserName = user.UserName;
        this.Email = user.Email;
        this.Phone = user.Phone?.Trim();
        this.Gender = user.Gender?.Trim();
        this.Password = user.Password?.Trim();
        this.StatusId = user.StatusId;
        this.AppUserClassroomMappings = user.AppUserClassroomMappings == null ? null : user.AppUserClassroomMappings.Select(x => new AppUser_AppUserClassroomMappingDTO(x)).ToList();
    }
}
