using CodeBE_LEM.Controllers.AppUserController;
using CodeBE_LEM.Entities;
using CodeBE_LEM.Models;
using CodeBE_LEM.Repositories;
using CodeBE_LEM.Services;
using CodeBE_LEM.Services.PermissionService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CodeBE_LEM.Controllers.PermissionController
{
    public class PermissionController : ControllerBase
    {
        private IPermissionService PermissionService;

        public PermissionController(IPermissionService PermissionService)
        {
            this.PermissionService = PermissionService;
        }

        [HttpPost, Route(PermissionRoute.ListPath), Authorize]
        public async Task<List<string>> ListPath([FromBody] Permission_AppUserClassroomMappingDTO Permission_AppUserClassroomMappingDTO)
        {
            AppUserClassroomMapping AppUserClassroomMapping = ConvertDTOToEntity(Permission_AppUserClassroomMappingDTO);
            List<string> Paths = await PermissionService.ListPath(AppUserClassroomMapping);
            return Paths;
        }

        [HttpPost, Route(PermissionRoute.ListAllPath), Authorize]
        public async Task<List<string>> ListAllPath()
        {
            List<string> Paths = await PermissionService.ListAllPath();
            return Paths;
        }

        [Route(PermissionRoute.ListPermission), HttpPost, Authorize]
        public async Task<ActionResult<List<Permission_PermissionDTO>>> ListPermission([FromBody] Permission_RoleDTO Permission_RoleDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await PermissionService.HasPermission(PermissionRoute.ListPermission, Permission_RoleDTO.ClassroomId))
            {
                return Forbid();
            }

            List<Permission> Permissions = await PermissionService.ListPermission();
            List<Permission_PermissionDTO> PermissionDTOs = Permissions.Select(x => new Permission_PermissionDTO(x)).ToList();
            return PermissionDTOs;
        }

        [Route(PermissionRoute.ListPermissionByRole), HttpPost, Authorize]
        public async Task<ActionResult<List<Permission_PermissionDTO>>> ListPermissionByRole([FromBody] Permission_RoleDTO Permission_RoleDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await PermissionService.HasPermission(PermissionRoute.ListPermissionByRole, Permission_RoleDTO.ClassroomId))
            {
                return Forbid();
            }

            List<Permission> Permissions = await PermissionService.ListPermissionByRole(Permission_RoleDTO.Id);
            List<Permission_PermissionDTO> PermissionDTOs = Permissions.Select(x => new Permission_PermissionDTO(x)).ToList();
            return PermissionDTOs;
        }


        [Route(PermissionRoute.ListRole), HttpPost, Authorize]
        public async Task<ActionResult<List<Permission_RoleDTO>>> ListRole([FromBody] Permission_RoleDTO Permission_RoleDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await PermissionService.HasPermission(PermissionRoute.ListRole, Permission_RoleDTO.ClassroomId))
            {
                return Forbid();
            }

            List<Role> Roles = Permission_RoleDTO.IsFull == true ? (await PermissionService.ListRole()) : (await PermissionService.ListRoleByClassId(Permission_RoleDTO.ClassroomId));
            List<Permission_RoleDTO> Permission_RoleDTOs = Roles.Select(x => new Permission_RoleDTO(x)).ToList();
            return Permission_RoleDTOs;
        }

        [Route(PermissionRoute.GetRole), HttpPost, Authorize]
        public async Task<ActionResult<Permission_RoleDTO>> GetRole([FromBody] Permission_RoleDTO Permission_RoleDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await PermissionService.HasPermission(PermissionRoute.GetRole, Permission_RoleDTO.ClassroomId))
            {
                return Forbid();
            }

            Role Role = ConvertPermission_RoleDTOToRoleEntity(Permission_RoleDTO);
            Role = await PermissionService.GetRole(Role.Id);
            Permission_RoleDTO = new Permission_RoleDTO(Role);
            return Permission_RoleDTO;
        }

        [Route(PermissionRoute.CreateRole), HttpPost, Authorize]
        public async Task<ActionResult<bool>> CreateRole([FromBody] Permission_RoleDTO Permission_RoleDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await PermissionService.HasPermission(PermissionRoute.CreateRole, Permission_RoleDTO.ClassroomId))
            {
                return Forbid();
            }

            Role Role = ConvertPermission_RoleDTOToRoleEntity(Permission_RoleDTO);
            bool isRegisterSuccess = await PermissionService.CreateRole(Role);
            if (isRegisterSuccess)
                return true;
            else
                return BadRequest("Create Role Fail");
        }

        [Route(PermissionRoute.UpdateRole), HttpPost, Authorize]
        public async Task<ActionResult<Permission_RoleDTO>> UpdateRole([FromBody] Permission_RoleDTO Permission_RoleDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await PermissionService.HasPermission(PermissionRoute.UpdateRole, Permission_RoleDTO.ClassroomId))
            {
                return Forbid();
            }

            Role Role = ConvertPermission_RoleDTOToRoleEntity(Permission_RoleDTO);
            Role = await PermissionService.UpdateRole(Role);
            Permission_RoleDTO = new Permission_RoleDTO(Role);
            if (Permission_RoleDTO != null)
                return Permission_RoleDTO;
            else
                return BadRequest(Permission_RoleDTO);
        }

        [Route(PermissionRoute.DeleteRole), HttpPost, Authorize]
        public async Task<ActionResult<Permission_RoleDTO>> DeleteRole([FromBody] Permission_RoleDTO Permission_RoleDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await PermissionService.HasPermission(PermissionRoute.DeleteRole, Permission_RoleDTO.ClassroomId))
            {
                return Forbid();
            }

            Role Role = ConvertPermission_RoleDTOToRoleEntity(Permission_RoleDTO);
            Role = await PermissionService.DeleteRole(Role);
            Permission_RoleDTO = new Permission_RoleDTO(Role);
            if (Permission_RoleDTO != null)
                return Permission_RoleDTO;
            else
                return BadRequest(Permission_RoleDTO);
        }

        private Role ConvertPermission_RoleDTOToRoleEntity(Permission_RoleDTO Permission_RoleDTO)
        {
            Role role = new Role();
            role.Id = Permission_RoleDTO.Id;
            role.Description = Permission_RoleDTO.Description;
            role.Name = Permission_RoleDTO.Name;
            role.PermissionRoleMappings = Permission_RoleDTO.PermissionRoleMappings?.Select(x => new PermissionRoleMapping
            {
                Id = x.Id,
                RoleId = x.RoleId,
                PermissionId = x.PermissionId
            }).ToList();
            return role;
        }

        private AppUserClassroomMapping ConvertDTOToEntity(Permission_AppUserClassroomMappingDTO Permission_AppUserClassroomMappingDTO)
        {
            AppUserClassroomMapping AppUserClassroomMapping = new AppUserClassroomMapping();
            AppUserClassroomMapping.Id = Permission_AppUserClassroomMappingDTO.Id;
            AppUserClassroomMapping.AppUserId = Permission_AppUserClassroomMappingDTO.AppUserId;
            AppUserClassroomMapping.ClassroomId = Permission_AppUserClassroomMappingDTO.ClassroomId;
            AppUserClassroomMapping.RoleId = Permission_AppUserClassroomMappingDTO.RoleId;
            return AppUserClassroomMapping;
        }
    }

}
