using CodeBE_LEM.Controllers.AppUserController;
using CodeBE_LEM.Controllers.PermissionController;
using CodeBE_LEM.Entities;
using CodeBE_LEM.Models;
using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Controllers.AppUserController;

public partial class AppUser_TokenDTO
{
    public string? Token { get; set; }
    public AppUser_TokenDTO() { }
    public AppUser_TokenDTO(string token)
    {
        this.Token = token;
    }
}
