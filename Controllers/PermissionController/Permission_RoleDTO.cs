using CodeBE_LEM.Entities;
using CodeBE_LEM.Models;
using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Controllers.PermissionController;

public partial class Permission_RoleDTO
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public long ClassroomId { get; set; }

    public List<Permisson_PermissonRoleMappingDTO>? PermissionRoleMappings { get; set; }

    public Permission_RoleDTO() { }

    public Permission_RoleDTO(Role Role)
    {
        this.Id = Role.Id;
        this.Name = Role.Name;
        this.Description = Role.Description;
        this.PermissionRoleMappings = Role.PermissionRoleMappings == null ? null : Role.PermissionRoleMappings.Select(x => new Permisson_PermissonRoleMappingDTO(x)).ToList();
    }
}

