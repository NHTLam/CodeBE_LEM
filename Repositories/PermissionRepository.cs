using CodeBE_LEM.Entities;
using CodeBE_LEM.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        Task<bool> CreateRole(Role Role);
        Task<bool> UpdateRole(Role Role);
        Task<bool> DeleteRole(Role Role);
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

        public async Task<List<string>> ListAllPath()
        {
            List<string> Paths = (await ListPermission()).Select(x => x.Path).ToList();
            return Paths;
        }

        public async Task Init(List<Permission> Permissions)
        {
            var PermissonRoleMappings = await DataContext.PermissionRoleMappings.AsNoTracking().ToListAsync();
            await DataContext.PermissionRoleMappings.Where(x => !Permissions.Select(p => p.Id).Contains(x.PermissionId)).DeleteFromQueryAsync();
            await DataContext.Permissions.Where(x => !Permissions.Select(p => p.Id).Contains(x.Id)).DeleteFromQueryAsync();
            await DataContext.BulkMergeAsync(Permissions);

            var PermissonIds = (await ListPermission()).Select(x => x.Id).ToList();
            PermissonRoleMappings = PermissonRoleMappings.Where(x => PermissonIds.Contains(x.PermissionId)).ToList();
            await DataContext.BulkMergeAsync(PermissonRoleMappings);
        }

        public async Task<List<Role>> ListRole()
        {
            IQueryable<RoleDAO> query = DataContext.Roles.AsNoTracking();
            List<Role> Roles = await query.AsNoTracking().Select(x => new Role
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
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
    }
}
