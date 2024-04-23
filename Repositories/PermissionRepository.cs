using CodeBE_LEM.Entities;
using CodeBE_LEM.Enums;
using CodeBE_LEM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Security;

namespace CodeBE_LEM.Repositories
{
    public interface IPermissionRepository
    {
        Task Init(List<Permission> Permissions);
        Task<List<Permission>> ListPermission();
        Task<List<string>> ListAllPath();
        Task<List<Role>> ListRole();
        Task<Role> GetRole(long Id);
        Task<List<Role>> ListSystemRole();
        Task<bool> CreateRole(Role Role);
        Task<bool> UpdateRole(Role Role);
        Task<bool> DeleteRole(Role Role);
        Task<bool> DeleteRoleAuto();
        Task<List<Role>> ListRoleByClassRoomAndUserId(long UserId, long ClassroomId);
    }

    public class PermissionRepository : IPermissionRepository
    {
        private DataContext DataContext;

        public PermissionRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        public async Task<List<Permission>> ListPermission()
        {
            IQueryable<PermissionDAO> query = DataContext.Permissions.AsNoTracking();
            List<Permission> Permissions = await query.AsNoTracking()
            .Select(x => new Permission
            {
                Id = x.Id,
                Path = x.Path,
                Name = x.Name,
                Description = x.Description,
                MenuName = x.MenuName,
            }).ToListAsync();
            return Permissions;
        }

        public async Task<List<Role>> ListRoleByClassRoomAndUserId(long UserId, long ClassroomId)
        {
            IQueryable<AppUserClassroomMappingDAO> query = DataContext.AppUserClassroomMappings.AsNoTracking()
                .Where(x => x.AppUserId == UserId && x.ClassroomId == ClassroomId);
            List<AppUserClassroomMapping> AppUserClassroomMappings = await query.AsNoTracking()
                .Select(x => new AppUserClassroomMapping
                {
                    Id = x.Id,
                    ClassroomId = x.ClassroomId,
                    AppUserId = x.AppUserId,
                    RoleId = x.RoleId,
                    Role = x.Role == null ? null : new Role
                    {
                        Id = x.Role.Id,
                        RoleTypeId = x.Role.RoleTypeId,
                        Description = x.Role.Description,
                        Name = x.Role.Name,
                    }
                }).ToListAsync();

            List<Role> Roles = AppUserClassroomMappings.Where(x => x.RoleId != null).DistinctBy(x => x.RoleId).Select(x => x.Role).ToList();
            return Roles;
        }

        public async Task<List<string>> ListAllPath()
        {
            List<string> Paths = (await ListPermission()).Select(x => x.Path).ToList();
            return Paths;
        }

        public async Task Init(List<Permission> Permissions)
        {
            List<PermissionDAO> permissionDAOs = Permissions.Select(ConvertEntityToDAO).ToList();
            var PermissonRoleMappings = await DataContext.PermissionRoleMappings.AsNoTracking().ToListAsync();
            await DataContext.PermissionRoleMappings.Where(x => !Permissions.Select(p => p.Id).Contains(x.PermissionId)).DeleteFromQueryAsync();
            await DataContext.Permissions.Where(x => !Permissions.Select(p => p.Id).Contains(x.Id)).DeleteFromQueryAsync();
            var AddPermissions = permissionDAOs.Where(x => x.Id == 0).ToList();
            var UpdatePermissions = permissionDAOs.Where(x => x.Id != 0).DistinctBy(x => x.Id).ToList();
            foreach (var permission in AddPermissions)
            {
                if (permission.Id == 0)
                {
                    DataContext.Permissions.Add(permission);
                }
            }

            foreach (var permission in UpdatePermissions)
            {
                if (permission.Id != 0)
                {
                    DataContext.Permissions.Update(permission);
                }
            }

            var PermissonIds = (await ListPermission()).Select(x => x.Id).ToList();
            PermissonRoleMappings = PermissonRoleMappings.Where(x => PermissonIds.Contains(x.PermissionId)).ToList();
            PermissonRoleMappings = PermissonRoleMappings.DistinctBy(x => x.Id).ToList();
            foreach (var PermissonRoleMapping in PermissonRoleMappings)
            {
                if (PermissonRoleMapping.Id == 0)
                {
                    DataContext.PermissionRoleMappings.Add(PermissonRoleMapping);
                }
                else
                {
                    DataContext.PermissionRoleMappings.Update(PermissonRoleMapping);
                }
            }
            await DataContext.SaveChangesAsync();
        }

        public async Task<List<Role>> ListRole()
        {
            IQueryable<RoleDAO> query = DataContext.Roles.AsNoTracking();
            List<Role> Roles = await query.AsNoTracking().Select(x => new Role
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                RoleTypeId = x.RoleTypeId,
            }).ToListAsync();

            var PermissionRoleMappingQuery = DataContext.PermissionRoleMappings.AsNoTracking();
            List<PermissionRoleMapping> PermissionRoleMappings = await PermissionRoleMappingQuery
                .Select(x => new PermissionRoleMapping
                {
                    Id = x.Id,
                    RoleId = x.RoleId,
                    PermissionId = x.PermissionId,
                }).ToListAsync();

            foreach (Role Role in Roles)
            {
                Role.PermissionRoleMappings = PermissionRoleMappings
                    .Where(x => x.Id == Role.Id)
                    .ToList();
            }

            return Roles;
        }

        public async Task<List<Role>> ListSystemRole()
        {
            IQueryable<RoleDAO> query = DataContext.Roles.AsNoTracking().Where(x => x.RoleTypeId == RoleTypeEnum.AUTO.Id);
            List<Role> Roles = await query.AsNoTracking().Select(x => new Role
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                RoleTypeId = x.RoleTypeId,
            }).ToListAsync();

            var PermissionRoleMappingQuery = DataContext.PermissionRoleMappings.AsNoTracking();
            List<PermissionRoleMapping> PermissionRoleMappings = await PermissionRoleMappingQuery
                .Select(x => new PermissionRoleMapping
                {
                    Id = x.Id,
                    RoleId = x.RoleId,
                    PermissionId = x.PermissionId,
                }).ToListAsync();

            foreach (Role Role in Roles)
            {
                Role.PermissionRoleMappings = PermissionRoleMappings
                    .Where(x => x.Id == Role.Id)
                    .ToList();
            }

            return Roles;
        }

        public async Task<Role> GetRole(long Id)
        {
            Role? Role = await DataContext.Roles.AsNoTracking()
            .Where(x => x.Id == Id)
            .Select(x => new Role()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                RoleTypeId = x.RoleTypeId,
            }).FirstOrDefaultAsync();

            if (Role == null)
                return null;
            Role.PermissionRoleMappings = await DataContext.PermissionRoleMappings.AsNoTracking()
                .Where(x => x.RoleId == Role.Id)
                .Select(x => new PermissionRoleMapping
                {
                    Id = x.Id,
                    RoleId = x.RoleId,
                    PermissionId = x.PermissionId,
                }).ToListAsync();

            return Role;
        }

        public async Task<bool> CreateRole(Role Role)
        {
            RoleDAO RoleDAO = new RoleDAO();
            RoleDAO.Name = Role.Name;
            RoleDAO.Description = Role.Description;
            RoleDAO.RoleTypeId = Role.RoleTypeId;
            DataContext.Roles.Add(RoleDAO);
            await DataContext.SaveChangesAsync();
            Role.Id = RoleDAO.Id;
            await SaveReference(Role);
            return true;
        }

        public async Task<bool> UpdateRole(Role Role)
        {
            RoleDAO? NewRole = DataContext.Roles
                .Where(x => x.Id == Role.Id)
                .FirstOrDefault();
            if (NewRole == null)
                return false;
            NewRole.Id = Role.Id;
            NewRole.Name = Role.Name;
            NewRole.RoleTypeId = Role.RoleTypeId;
            NewRole.Description = Role.Description;
            await DataContext.SaveChangesAsync();
            await SaveReference(Role);
            return true;
        }

        public async Task<bool> DeleteRole(Role Role)
        {
            RoleDAO? RoleDAO = DataContext.Roles
                .Where(x => x.Id == Role.Id)
                .FirstOrDefault();
            if (Role == null)
                return false;
            DataContext.Roles.Remove(RoleDAO);
            await DataContext.SaveChangesAsync();
            await SaveReference(Role);
            return true;
        }

        public async Task<bool> DeleteRoleAuto()
        {
            await DataContext.Roles.Where(x => x.RoleTypeId == RoleTypeEnum.AUTO.Id)
                .DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(Role Role)
        {
            if (Role.PermissionRoleMappings == null || Role.PermissionRoleMappings.Count == 0)
                await DataContext.PermissionRoleMappings
                    .Where(x => x.RoleId == Role.Id)
                    .DeleteFromQueryAsync();
            else
            {
                var PermissionRoleMappingIds = Role.PermissionRoleMappings.Select(x => x.Id).Distinct().ToList();
                await DataContext.PermissionRoleMappings
                    .Where(x => x.RoleId == Role.Id)
                    .Where(x => !PermissionRoleMappingIds.Contains(x.Id))
                    .DeleteFromQueryAsync();

                List<PermissionRoleMappingDAO> PermissionRoleMappingDAOs = new List<PermissionRoleMappingDAO>();
                foreach (PermissionRoleMapping PermissionRoleMapping in Role.PermissionRoleMappings)
                {
                    PermissionRoleMappingDAO PermissionRoleMappingDAO = new PermissionRoleMappingDAO();
                    PermissionRoleMappingDAO.RoleId = Role.Id;
                    PermissionRoleMappingDAO.PermissionId = PermissionRoleMapping.PermissionId;
                    PermissionRoleMappingDAOs.Add(PermissionRoleMappingDAO);
                }
                await DataContext.BulkMergeAsync(PermissionRoleMappingDAOs);
            }
        }

        private PermissionDAO ConvertEntityToDAO(Permission Permission)
        {
            PermissionDAO PermissionDAO = new PermissionDAO();
            PermissionDAO.Id = Permission.Id;
            PermissionDAO.Name = Permission.Name;
            PermissionDAO.Path = Permission.Path;
            PermissionDAO.Description = Permission.Description;
            PermissionDAO.MenuName = Permission.MenuName;
            return PermissionDAO;
        }
    }
}
