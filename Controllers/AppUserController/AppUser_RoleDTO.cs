using CodeBE_LEM.Controllers.AppUserController;
using CodeBE_LEM.Controllers.PermissionController;
using CodeBE_LEM.Entities;
using CodeBE_LEM.Models;
using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Controllers.AppUserController;

public partial class AppUser_RoleDTO
{
    public long Id { get; set; }
    public string Name { get; set; }
    public AppUser_RoleDTO() { }
    public AppUser_RoleDTO(Role Role)
    {
        this.Id = Role.Id;
        this.Name = Role.Name;
    }
}
