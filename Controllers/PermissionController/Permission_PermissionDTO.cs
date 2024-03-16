using CodeBE_LEM.Entities;
using CodeBE_LEM.Models;
using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Controllers.PermissionController;

public class Permission_PermissionDTO
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Path { get; set; } = null!;

    public string? Description { get; set; }

    public string? MenuName { get; set; }

    public Permission_PermissionDTO() { }

    public Permission_PermissionDTO(Permission permission)
    {
        this.Id = permission.Id;
        this.Name = permission.Name;
        this.Path = permission.Path;
        this.Description = permission.Description;
        this.MenuName = permission.MenuName;
    }
}

