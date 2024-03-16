using CodeBE_LEM.Entities;
using CodeBE_LEM.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;

namespace CodeBE_LEM.Repositories
{
    public interface IAppUserRepository
    {
        Task<List<AppUser>> List();
        Task<AppUser> Get(long Id);
        Task<bool> Create(AppUser AppUser);
        Task<bool> Update(AppUser AppUser);
        Task<bool> Delete(AppUser AppUser);
    }

    public class AppUserRepository : IAppUserRepository
    {
        private DataContext DataContext;

        public AppUserRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        public async Task<List<AppUser>> List()
        {
            IQueryable<AppUserDAO> query = DataContext.AppUsers.AsNoTracking();
            List<AppUser> AppUsers = await query.AsNoTracking().Select(x => new AppUser
            {
                Id = x.Id,
                FullName = x.FullName,
                UserName = x.UserName,
                Email = x.Email,
                Phone = x.Phone,
                Gender = x.Gender,
                Password = x.Password,
                StatusId = x.StatusId,
            }).ToListAsync();

            var AppUserRoleMappingQuery = DataContext.AppUserRoleMappings.AsNoTracking();
            List<AppUserRoleMapping> AppUserRoleMappings = await AppUserRoleMappingQuery
                .Select(x => new AppUserRoleMapping
                {
                    Id = x.Id,
                    RoleId = x.RoleId,
                    AppUserId = x.AppUserId,
                }).ToListAsync();

            foreach (AppUser AppUser in AppUsers)
            {
                AppUser.AppUserRoleMappings = AppUserRoleMappings
                    .Where(x => x.AppUserId == AppUser.Id)
                    .ToList();
            }

            return AppUsers;
        }

        public async Task<AppUser> Get(long Id)
        {
            AppUser? AppUser = await DataContext.AppUsers.AsNoTracking()
            .Where(x => x.Id == Id)
            .Select(x => new AppUser()
            {
                Id = x.Id,
                FullName = x.FullName,
                UserName = x.UserName,
                Email = x.Email,
                Phone = x.Phone,
                Gender = x.Gender,
                Password = x.Password,
                StatusId = x.StatusId,
            }).FirstOrDefaultAsync();

            if (AppUser == null)
                return null;
            AppUser.AppUserRoleMappings = await DataContext.AppUserRoleMappings.AsNoTracking()
                .Where(x => x.AppUserId == AppUser.Id)
                .Select(x => new AppUserRoleMapping
                {
                    Id = x.Id,
                    AppUserId = x.AppUserId,
                    RoleId = x.RoleId,
                }).ToListAsync();

            return AppUser;
        }

        public async Task<bool> Create(AppUser AppUser)
        {
            AppUserDAO AppUserDAO = new AppUserDAO();
            AppUserDAO.Id = AppUser.Id;
            AppUserDAO.FullName = AppUser.FullName;
            AppUserDAO.UserName = AppUser.UserName;
            AppUserDAO.Email = AppUser.Email;
            AppUserDAO.Phone = AppUser.Phone;
            AppUserDAO.Gender = AppUser.Gender;
            AppUserDAO.Password = AppUser.Password;
            AppUserDAO.StatusId = AppUser.StatusId;
            DataContext.AppUsers.Add(AppUserDAO);
            await DataContext.SaveChangesAsync();
            AppUser.Id = AppUserDAO.Id;
            await SaveReference(AppUser);
            return true;
        }

        public async Task<bool> Update(AppUser AppUser)
        {
            AppUserDAO? AppUserDAO = DataContext.AppUsers
                .Where(x => x.Id == AppUser.Id)
                .FirstOrDefault();
            if (AppUserDAO == null)
                return false;
            AppUserDAO.Id = AppUser.Id;
            AppUserDAO.FullName = AppUser.FullName;
            AppUserDAO.UserName = AppUser.UserName;
            AppUserDAO.Email = AppUser.Email;
            AppUserDAO.Phone = AppUser.Phone;
            AppUserDAO.Gender = AppUser.Gender;
            AppUserDAO.Password = AppUser.Password;
            AppUserDAO.StatusId = AppUser.StatusId;
            await DataContext.SaveChangesAsync();
            await SaveReference(AppUser);
            return true;
        }

        public async Task<bool> Delete(AppUser AppUser)
        {
            AppUserDAO? AppUserDAO = DataContext.AppUsers
                .Where(x => x.Id == AppUser.Id)
                .FirstOrDefault();
            if (AppUser == null)
                return false;
            DataContext.AppUsers.Remove(AppUserDAO);
            await DataContext.SaveChangesAsync();
            await SaveReference(AppUser);
            return true;
        }

        private async Task SaveReference(AppUser AppUser)
        {
            if (AppUser.AppUserRoleMappings == null || AppUser.AppUserRoleMappings.Count == 0)
                await DataContext.AppUserRoleMappings
                    .Where(x => x.AppUserId == AppUser.Id)
                    .DeleteFromQueryAsync();
            else
            {
                var AppUserRoleMappingIds = AppUser.AppUserRoleMappings.Select(x => x.Id).Distinct().ToList();
                await DataContext.AppUserRoleMappings
                    .Where(x => x.AppUserId == AppUser.Id)
                    .Where(x => !AppUserRoleMappingIds.Contains(x.Id))
                    .DeleteFromQueryAsync();

                List<AppUserRoleMappingDAO> AppUserRoleMappingDAOs = new List<AppUserRoleMappingDAO>();
                foreach (AppUserRoleMapping PermissonAppUserMapping in AppUser.AppUserRoleMappings)
                {
                    AppUserRoleMappingDAO AppUserRoleMappingDAO = new AppUserRoleMappingDAO();
                    AppUserRoleMappingDAO.AppUserId = AppUser.Id;
                    AppUserRoleMappingDAO.RoleId = PermissonAppUserMapping.RoleId;
                    AppUserRoleMappingDAOs.Add(AppUserRoleMappingDAO);
                }
                await DataContext.BulkMergeAsync(AppUserRoleMappingDAOs);
            }
        }
    }
}
