using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using CodeBE_LEM.Models;
using CodeBE_LEM.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CodeBE_LEM.Entities;
using CodeBE_LEM.Controllers.AppUserController;
using CodeBE_LEM.Controllers.PermissionController;
using CodeBE_LEM.Controllers.AttachmentController;
using CodeBE_LEM.Controllers.BoardController;
using CodeBE_LEM.Controllers.ClassroomController;
using CodeBE_LEM.Controllers.JobController;
using CodeBE_LEM.Enums;

namespace CodeBE_LEM.Services.PermissionService
{
    public interface IPermissionService
    {
        Task Init();
        Task<List<string>> ListPath(AppUserClassroomMapping AppUserClassroomMapping);
        Task<List<string>> ListAllPath();
        Task<List<Role>> ListRole();
        Task<List<Role>> ListRoleByClassId(long ClassroomId);
        Task<List<Permission>> ListPermission();
        Task<List<Permission>> ListPermissionByRole(long RoleId);
        Task<Role> GetRole(long Id);
        Task<bool> CreateRole (Role Role, long? ClassroomId);
        Task<Role> UpdateRole(Role Role, long? ClassroomId);
        Task<Role> DeleteRole(Role Role);
        Task<bool> HasPermission(string Path, long AppUserId);
        long GetAppUserId();
    }
    public class PermissionService : IPermissionService
    {
        private IUOW UOW;
        private readonly IConfiguration Configuration;
        private IHttpContextAccessor HttpContextAccessor;

        public PermissionService(
            IUOW UOW,
            IConfiguration Configuration,
            IHttpContextAccessor HttpContextAccessor
        )
        {
            this.UOW = UOW;
            this.Configuration = Configuration;
            this.HttpContextAccessor = HttpContextAccessor;
        }

        public async Task Init()
        {
            try
            {
                var PermissionDbs = await UOW.PermissionRepository.ListPermission();
                Dictionary<string, List<string>> DictionaryPaths = new Dictionary<string, List<string>>();
                DictionaryPaths = ConcatMyDictionaryRoute(DictionaryPaths, AttachmentRoute.DictionaryPath);
                DictionaryPaths = ConcatMyDictionaryRoute(DictionaryPaths, BoardRoute.DictionaryPath);
                DictionaryPaths = ConcatMyDictionaryRoute(DictionaryPaths, ClassroomRoute.DictionaryPath);
                DictionaryPaths = ConcatMyDictionaryRoute(DictionaryPaths, JobRoute.DictionaryPath);
                DictionaryPaths = ConcatMyDictionaryRoute(DictionaryPaths, PermissionRoute.DictionaryPath);
                List<Permission> Permissions = new List<Permission>();
                foreach (var DictionaryPath in DictionaryPaths)
                {
                    foreach (var path in DictionaryPath.Value)
                    {
                        string modelName = path.Split('/')[2];
                        long? PermissionId = PermissionDbs.FirstOrDefault(x => x.Path.Trim() == path.Trim() && x.Name == DictionaryPath.Key)?.Id;
                        Permission permission = new Permission();
                        permission.Id = PermissionId ?? 0;
                        permission.Name = DictionaryPath.Key;
                        permission.Path = path;
                        permission.Description = $"Cho phép thực hiện hành động {DictionaryPath.Key} ở màn {modelName}";
                        permission.MenuName = modelName;
                        Permissions.Add(permission);
                    }
                }
                await UOW.PermissionRepository.Init(Permissions);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<List<string>> ListPath(AppUserClassroomMapping AppUserClassroomMapping)
        {
            try
            {
                List<long> RoleIds = await UOW.PermissionRepository.ListRoleByClassRoomAndUserId(GetAppUserId(), AppUserClassroomMapping.ClassroomId);
                List<Role> Roles = await UOW.PermissionRepository.ListRole();
                Roles = Roles.Where(x => RoleIds.Contains(x.Id)).ToList();

                List<long> permissionIds = new List<long>();
                foreach (var Role in Roles)
                {
                    if (Role.PermissionRoleMappings != null && Role.PermissionRoleMappings.Count > 0)
                        permissionIds.AddRange(Role.PermissionRoleMappings.Select(x => x.PermissionId).ToList());
                }
                permissionIds = permissionIds.Distinct().ToList();

                List<Permission> AllowPermission = await UOW.PermissionRepository.ListPermission();
                List<string> AllowPath = AllowPermission.Where(x => permissionIds.Contains(x.Id)).Select(x => x.Path).ToList();
                return AllowPath;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<List<Permission>> ListPermission()
        {
            try
            {
                return await UOW.PermissionRepository.ListPermission();
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<List<Permission>> ListPermissionByRole(long RoleId)
        {
            try
            {
                Role Role = await UOW.PermissionRepository.GetRole(RoleId);
                List<long> PermissionIds = Role.PermissionRoleMappings.Select(x => x.PermissionId).ToList();
                List<Permission> Permissions = await UOW.PermissionRepository.ListPermission();
                Permissions = Permissions.Where(x => PermissionIds.Contains(x.Id)).ToList();
                return Permissions;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<bool> HasPermission(string Path, long ClassroomId)
        {
            AppUserClassroomMapping appUserClassroomMapping = new AppUserClassroomMapping();
            appUserClassroomMapping.ClassroomId = ClassroomId;
            List<string> AllowPath = await ListPath(appUserClassroomMapping);
            if (AllowPath.Contains(Path))
            {
                return true;
            }
            return false;
        }

        public async Task<List<string>> ListAllPath()
        {
            try
            {
                return await UOW.PermissionRepository.ListAllPath();
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<bool> CreateRole(Role Role, long? classroomId)
        {
            try
            {
                Role.RoleTypeId = RoleTypeEnum.USER_CREATED.Id;
                await UOW.PermissionRepository.CreateRole(Role);
                Role = await UOW.PermissionRepository.GetRole(Role.Id);
                if (classroomId != null)
                {
                    await UOW.PermissionRepository.AssignRoleInClass(Role.Id, classroomId.Value, GetAppUserId());
                }
                return true;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<Role> DeleteRole(Role Role)
        {
            try
            {
                await UOW.PermissionRepository.DeleteRole(Role);
                return Role;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task<Role> GetRole(long Id)
        {
            Role Role = await UOW.PermissionRepository.GetRole(Id);
            if (Role == null)
                return null;
            return Role;
        }

        public async Task<List<Role>> ListRole()
        {
            try
            {
                List<Role> Roles = await UOW.PermissionRepository.ListRole();
                return Roles;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<List<Role>> ListRoleByClassId(long classroomId)
        {
            try
            {
                List<Role> FullRoles = await UOW.PermissionRepository.ListRole();
                List<Role> Roles = await UOW.PermissionRepository.ListRoleByClassId(classroomId);
                List<Role> SystemRoles = await UOW.PermissionRepository.ListSystemRole();
                Roles.AddRange(SystemRoles);
                Roles = Roles.DistinctBy(x => x.Id).ToList();

                Roles = FullRoles.Where(x => FullRoles.Select(x => x.Id).Contains(x.Id)).ToList();
                return Roles;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<Role> UpdateRole(Role Role, long? classroomId)
        {
            try
            {
                var oldData = await UOW.PermissionRepository.GetRole(Role.Id);

                Role.RoleTypeId = RoleTypeEnum.USER_CREATED.Id;
                await UOW.PermissionRepository.UpdateRole(Role);

                Role = await UOW.PermissionRepository.GetRole(Role.Id);
                if (classroomId != null)
                {
                    await UOW.PermissionRepository.AssignRoleInClass(Role.Id, classroomId.Value, GetAppUserId());
                }
                return Role;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        private Dictionary<string, List<string>> ConcatMyDictionaryRoute(Dictionary<string, List<string>> CurrentDictionaryPath, Dictionary<string, List<string>> NewDictionaryPath)
        {
            if (CurrentDictionaryPath != null && CurrentDictionaryPath.Count != 0)
            {
                foreach (var Dictionary in NewDictionaryPath)
                {
                    if (CurrentDictionaryPath.Select(x => x.Key).ToList().Contains(Dictionary.Key))
                    {
                        CurrentDictionaryPath[Dictionary.Key].AddRange(Dictionary.Value);
                    }
                    else
                    {
                        CurrentDictionaryPath.Add(Dictionary.Key, Dictionary.Value);
                    }
                }
                return CurrentDictionaryPath;
            }
            else
            {
                return NewDictionaryPath;
            }
        }

        public long GetAppUserId()
        {
            int AppUserId = 0;
            if (HttpContextAccessor.HttpContext is not null)
            {
                AppUserId = int.TryParse(HttpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier), out int id) ? id : 0;
            }
            return AppUserId;
        }
    }
}
