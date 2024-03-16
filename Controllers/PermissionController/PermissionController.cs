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
        public async Task<List<string>> ListPath([FromBody] AppUser_AppUserDTO UserDTO)
        {
            AppUser User = ConvertUserDTOToUserEntity(UserDTO);
            List<string> Paths = await PermissionService.ListPath(User);
            return Paths;
        }

        [HttpPost, Route(PermissionRoute.ListAllPath), Authorize]
        public async Task<List<string>> ListAllPath()
        {
            List<string> Paths = await PermissionService.ListAllPath();
            return Paths;
        }

        [HttpGet, Route(PermissionRoute.Init)]
        public async Task Init()
        {
            await PermissionService.Init();
        }

        [Route(PermissionRoute.ListRole), HttpPost, Authorize]
        public async Task<ActionResult<List<Permission_RoleDTO>>> ListRole()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await PermissionService.HasPermission(PermissionRoute.ListRole, PermissionService.GetAppUserId()))
            {
                return Forbid();
            }

            List<Role> Roles = await PermissionService.ListRole();
            List<Permission_RoleDTO> Permission_RoleDTOs = Roles.Select(x => new Permission_RoleDTO(x)).ToList();
            return Permission_RoleDTOs;
        }

        [Route(PermissionRoute.GetRole), HttpPost, Authorize]
        public async Task<ActionResult<Permission_RoleDTO>> GetRole([FromBody] Permission_RoleDTO Permission_RoleDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await PermissionService.HasPermission(PermissionRoute.GetRole, PermissionService.GetAppUserId()))
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

            if (!await PermissionService.HasPermission(PermissionRoute.CreateRole, PermissionService.GetAppUserId()))
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

            if (!await PermissionService.HasPermission(PermissionRoute.UpdateRole, PermissionService.GetAppUserId()))
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

            if (!await PermissionService.HasPermission(PermissionRoute.DeleteRole, PermissionService.GetAppUserId()))
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

        private AppUser ConvertUserDTOToUserEntity(AppUser_AppUserDTO AppUser_AppUserDTO)
        {
            AppUser AppUser = new AppUser();
            AppUser.Id = AppUser_AppUserDTO.Id;
            AppUser.FullName = AppUser_AppUserDTO.FullName;
            AppUser.UserName = AppUser_AppUserDTO.UserName;
            AppUser.Email = AppUser_AppUserDTO.Email;
            AppUser.Gender = AppUser_AppUserDTO.Gender;
            AppUser.Phone = AppUser_AppUserDTO.Phone;
            AppUser.StatusId = AppUser_AppUserDTO.StatusId ?? 0;

            return AppUser;
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
    }

}
