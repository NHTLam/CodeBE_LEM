﻿using CodeBE_LEM.Entities;
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

            var AppUserClassroomMappingQuery = DataContext.AppUserClassroomMappings.AsNoTracking();
            List<AppUserClassroomMapping> AppUserClassroomMappings = await AppUserClassroomMappingQuery
                .Select(x => new AppUserClassroomMapping
                {
                    Id = x.Id,
                    ClassroomId = x.ClassroomId,
                    AppUserId = x.AppUserId,
                    RoleId = x.RoleId,
                    Role = x.Role == null ? null : new Role
                    {
                        Id = x.Role.Id,
                        Name = x.Role.Name,
                        Description = x.Role.Description,
                        RoleTypeId = x.Role.RoleTypeId,
                    }
                }).ToListAsync();

            foreach (AppUser AppUser in AppUsers)
            {
                AppUser.AppUserClassroomMappings = AppUserClassroomMappings
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

            AppUser.AppUserClassroomMappings = await DataContext.AppUserClassroomMappings.AsNoTracking()
                .Where(x => x.AppUserId == AppUser.Id)
                .Select(x => new AppUserClassroomMapping
                {
                    Id = x.Id,
                    ClassroomId = x.ClassroomId,
                    AppUserId = x.AppUserId,
                    RoleId = x.RoleId,
                    Role = x.Role == null ? null : new Role
                    {
                        Id = x.Role.Id,
                        Name = x.Role.Name,
                        Description = x.Role.Description,
                        RoleTypeId = x.Role.RoleTypeId,
                    }
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
            //await SaveReference(AppUser);
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
            if (AppUser.AppUserClassroomMappings == null || AppUser.AppUserClassroomMappings.Count == 0)
            {
                await DataContext.AppUserClassroomMappings
                    .Where(x => x.AppUserId == AppUser.Id)
                    .DeleteFromQueryAsync();
            }
            else
            {
                var AppUserClassroomMappingIds = AppUser.AppUserClassroomMappings.Select(x => x.Id).Distinct().ToList();
                await DataContext.AppUserClassroomMappings
                    .Where(x => x.AppUserId == AppUser.Id)
                    .Where(x => !AppUserClassroomMappingIds.Contains(x.Id))
                    .DeleteFromQueryAsync();

                List<AppUserClassroomMappingDAO> AppUserClassroomMappingDAOs = new List<AppUserClassroomMappingDAO>();
                foreach (AppUserClassroomMapping AppUserClassroomMapping in AppUser.AppUserClassroomMappings)
                {
                    AppUserClassroomMappingDAO AppUserClassroomMappingDAO = new AppUserClassroomMappingDAO();
                    AppUserClassroomMappingDAO.Id = AppUserClassroomMapping.Id;
                    AppUserClassroomMappingDAO.ClassroomId = AppUserClassroomMapping.ClassroomId;
                    AppUserClassroomMappingDAO.AppUserId = AppUser.Id;
                    AppUserClassroomMappingDAO.RoleId = AppUserClassroomMapping.RoleId;
                    AppUserClassroomMappingDAOs.Add(AppUserClassroomMappingDAO);
                }
                await DataContext.AppUserClassroomMappings.AddRangeAsync(AppUserClassroomMappingDAOs);
            }
            await DataContext.SaveChangesAsync();
        }
    }
}
