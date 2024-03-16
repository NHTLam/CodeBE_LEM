using CodeBE_LEM.Entities;
using CodeBE_LEM.Models;
using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Controllers.PermissionController;

public class Permisson_PermissonRoleMappingDTO
{
    public long RoleId { get; set; }

    public long PermissionId { get; set; }

    public long Id { get; set; }

    public Permission_PermissionDTO Permission { get; set; }

    public Permisson_PermissonRoleMappingDTO() { }

    public Permisson_PermissonRoleMappingDTO(PermissionRoleMapping PermissonRoleMapping)
    {
        this.RoleId = PermissonRoleMapping.RoleId;
        this.PermissionId = PermissonRoleMapping.PermissionId;
        this.Id = PermissonRoleMapping.Id;
        this.Permission = PermissonRoleMapping.Permission == null ? null : new Permission_PermissionDTO(PermissonRoleMapping.Permission);
    }

}

