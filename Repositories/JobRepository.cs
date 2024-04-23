using CodeBE_LEM.Entities;
using CodeBE_LEM.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;

namespace CodeBE_LEM.Repositories
{
    public interface IJobRepository
    {
        Task<List<Job>> List();
        Task<List<Job>> List(List<long> Ids);
        Task<Job> Get(long Id);
        Task<bool> Create(Job Job);
        Task<bool> Update(Job Job);
        Task<bool> Delete(Job Job);
        Task<bool> BulkMerge(List<Job> Jobs);
        Task<bool> BulkDelete(List<Job> Jobs);

        Task<List<Job>> ListByCardIds(List<long> CardIds);
        Task<List<long>> ListJobIdByUserId(long CurrentUserId);
    }

    public class JobRepository : IJobRepository
    {
        private DataContext DataContext;

        public JobRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        public async Task<List<Job>> List()
        {
            IQueryable<JobDAO> query = DataContext.Jobs.AsNoTracking();
            List<Job> Jobs = await query.AsNoTracking()
            .Where(x => x.DeleteAt == null)
            .Select(x => new Job
            {
                Id = x.Id,
                CardId = x.CardId,
                Name = x.Name,
                Description = x.Description,
                Order = x.Order,
                StartAt = x.StartAt,
                EndAt = x.EndAt,
                Color = x.Color,
                NoTodoDone = x.NoTodoDone,
                IsAllDay = x.IsAllDay,
                CreatorId = x.CreatorId,
                CreatedAt = x.CreatedAt,
                UpdateAt = x.UpdateAt,
                Creator = x.Creator == null ? null : new AppUser
                {
                    Id = x.Creator.Id,
                    UserName = x.Creator.UserName,
                },
                Card = new Card
                {
                    Id = x.Card.Id,
                    Name = x.Card.Name,
                    BoardId = x.Card.BoardId,
                    Order = x.Card.Order,
                    CreatedAt = x.Card.CreatedAt,
                    UpdatedAt = x.Card.UpdatedAt
                },
            }).ToListAsync();

            var TodoQuery = DataContext.Todos.AsNoTracking();
            List<Todo> Todos = await TodoQuery
                .Select(x => new Todo
                {
                    Id = x.Id,
                    Description = x.Description,
                    JobId = x.JobId,
                    IsDone = x.IsDone,
                }).ToListAsync();

            foreach (Job Job in Jobs)
            {
                Job.Todos = Todos
                    .Where(x => x.JobId == Job.Id)
                    .ToList();
            }

            var AppUserJobMappingQuery = DataContext.AppUserJobMappings.AsNoTracking();
            List<AppUserJobMapping> AppUserJobMappings = await AppUserJobMappingQuery
                .Select(x => new AppUserJobMapping
                {
                    Id = x.Id,
                    AppUserId = x.AppUserId,
                    JobId = x.JobId,
                }).ToListAsync();

            foreach (Job Job in Jobs)
            {
                Job.AppUserJobMappings = AppUserJobMappings
                    .Where(x => x.JobId == Job.Id)
                    .ToList();
            }

            return Jobs;
        }

        public async Task<List<Job>> ListByCardIds(List<long> CardIds)
        {
            IQueryable<JobDAO> query = DataContext.Jobs.AsNoTracking();
            List<Job> Jobs = await query.AsNoTracking()
            .Where(x => x.DeleteAt == null)
            .Where(x => CardIds.Contains(x.CardId))
            .Select(x => new Job
            {
                Id = x.Id,
                CardId = x.CardId,
                Name = x.Name,
                Description = x.Description,
                Order = x.Order,
                StartAt = x.StartAt,
                EndAt = x.EndAt,
                Color = x.Color,
                NoTodoDone = x.NoTodoDone,
                IsAllDay = x.IsAllDay,
                CreatorId = x.CreatorId,
                CreatedAt = x.CreatedAt,
                UpdateAt = x.UpdateAt,
                Creator = x.Creator == null ? null : new AppUser
                {
                    Id = x.Creator.Id,
                    UserName = x.Creator.UserName,
                },
            }).ToListAsync();

            var TodoQuery = DataContext.Todos.AsNoTracking();
            List<Todo> Todos = await TodoQuery
                .Select(x => new Todo
                {
                    Id = x.Id,
                    Description = x.Description,
                    JobId = x.JobId,
                    IsDone = x.IsDone,
                }).ToListAsync();

            foreach (Job Job in Jobs)
            {
                Job.Todos = Todos
                    .Where(x => x.JobId == Job.Id)
                    .ToList();
            }

            var AppUserJobMappingQuery = DataContext.AppUserJobMappings.AsNoTracking();
            List<AppUserJobMapping> AppUserJobMappings = await AppUserJobMappingQuery
                .Select(x => new AppUserJobMapping
                {
                    Id = x.Id,
                    AppUserId = x.AppUserId,
                    JobId = x.JobId,
                }).ToListAsync();

            foreach (Job Job in Jobs)
            {
                Job.AppUserJobMappings = AppUserJobMappings
                    .Where(x => x.JobId == Job.Id)
                    .ToList();
            }

            return Jobs;
        }

        public async Task<List<long>> ListJobIdByUserId(long CurrentUserId)
        {
            IQueryable<AppUserJobMappingDAO> query = DataContext.AppUserJobMappings.AsNoTracking();
            List<AppUserJobMapping> AppUserJobMappings = await query.AsNoTracking()
            .Where(x => x.AppUserId == CurrentUserId)
            .Select(x => new AppUserJobMapping
            {
                Id = x.Id,
                AppUserId = x.AppUserId,
                JobId = x.JobId,
            }).ToListAsync();

            return AppUserJobMappings.Select(x => x.JobId).ToList();
        }

        public async Task<List<Job>> List(List<long> Ids)
        {
            IQueryable<JobDAO> query = DataContext.Jobs.AsNoTracking();
            List<Job> Jobs = await query.AsNoTracking()
            .Where(x => x.DeleteAt == null)
            .Where(q => Ids.Contains(q.Id))
            .Select(x => new Job
            {
                Id = x.Id,
                CardId = x.CardId,
                Name = x.Name,
                Description = x.Description,
                Order = x.Order,
                StartAt = x.StartAt,
                EndAt = x.EndAt,
                Color = x.Color,
                NoTodoDone = x.NoTodoDone,
                IsAllDay = x.IsAllDay,
                CreatorId = x.CreatorId,
                CreatedAt = x.CreatedAt,
                UpdateAt = x.UpdateAt,
                Creator = x.Creator == null ? null : new AppUser
                {
                    Id = x.Creator.Id,
                    UserName = x.Creator.UserName,
                },
                Card = new Card
                {
                    Id = x.Card.Id,
                    Name = x.Card.Name,
                    BoardId = x.Card.BoardId,
                    Order = x.Card.Order,
                    CreatedAt = x.Card.CreatedAt,
                    UpdatedAt = x.Card.UpdatedAt
                },
            }).ToListAsync();

            var TodoQuery = DataContext.Todos.AsNoTracking();
            List<Todo> Todos = await TodoQuery
                .Select(x => new Todo
                {
                    Id = x.Id,
                    Description = x.Description,
                    JobId = x.JobId,
                    IsDone = x.IsDone,
                }).ToListAsync();

            foreach (Job Job in Jobs)
            {
                Job.Todos = Todos
                    .Where(x => x.JobId == Job.Id)
                    .ToList();
            }

            var AppUserJobMappingQuery = DataContext.AppUserJobMappings.AsNoTracking();
            List<AppUserJobMapping> AppUserJobMappings = await AppUserJobMappingQuery
                .Select(x => new AppUserJobMapping
                {
                    Id = x.Id,
                    AppUserId = x.AppUserId,
                    JobId = x.JobId,
                }).ToListAsync();

            foreach (Job Job in Jobs)
            {
                Job.AppUserJobMappings = AppUserJobMappings
                    .Where(x => x.JobId == Job.Id)
                    .ToList();
            }

            return Jobs;
        }

        public async Task<Job> Get(long Id)
        {
            Job? Job = await DataContext.Jobs.AsNoTracking()
            .Where(x => x.DeleteAt == null)
            .Where(x => x.Id == Id)
            .Select(x => new Job()
            {
                Id = x.Id,
                CardId = x.CardId,
                Name = x.Name,
                Description = x.Description,
                Order = x.Order,
                StartAt = x.StartAt,
                EndAt = x.EndAt,
                Color = x.Color,
                NoTodoDone = x.NoTodoDone,
                IsAllDay = x.IsAllDay,
                CreatorId = x.CreatorId,
                CreatedAt = x.CreatedAt,
                UpdateAt = x.UpdateAt,
                Creator = x.Creator == null ? null : new AppUser
                {
                    Id = x.Creator.Id,
                    UserName = x.Creator.UserName,
                },
                Card = new Card
                {
                    Id = x.Card.Id,
                    Name = x.Card.Name,
                    BoardId = x.Card.BoardId,
                    Order = x.Card.Order,
                    CreatedAt = x.Card.CreatedAt,
                    UpdatedAt = x.Card.UpdatedAt
                },
            }).FirstOrDefaultAsync();

            if (Job == null)
                return new Job();
            Job.Todos = await DataContext.Todos.AsNoTracking()
                .Where(x => x.JobId == Job.Id)
                .Select(x => new Todo
                {
                    Id = x.Id,
                    Description = x.Description,
                    JobId = x.JobId,
                    IsDone = x.IsDone,
                }).ToListAsync();
            Job.AppUserJobMappings = await DataContext.AppUserJobMappings.AsNoTracking()
                .Where(x => x.JobId == Job.Id)
                .Select(x => new AppUserJobMapping
                {
                    Id = x.Id,
                    AppUserId = x.AppUserId,
                    JobId = x.JobId,
                }).ToListAsync();

            return Job;
        }

        public async Task<bool> Create(Job Job)
        {
            JobDAO JobDAO = new JobDAO();
            JobDAO.CardId = Job.CardId;
            JobDAO.Name = Job.Name;
            JobDAO.Description = Job.Description;
            JobDAO.Order = Job.Order;
            JobDAO.StartAt = Job.StartAt;
            JobDAO.EndAt = Job.EndAt;
            JobDAO.Color = Job.Color;
            JobDAO.NoTodoDone = Job.NoTodoDone;
            JobDAO.IsAllDay = Job.IsAllDay;
            JobDAO.CreatorId = Job.CreatorId;
            JobDAO.CreatedAt = DateTime.Now;
            JobDAO.UpdateAt = DateTime.Now;
            DataContext.Jobs.Add(JobDAO);
            await DataContext.SaveChangesAsync();
            Job.Id = JobDAO.Id;
            await SaveReference(Job);
            return true;
        }

        public async Task<bool> Update(Job Job)
        {
            JobDAO? JobDAO = DataContext.Jobs
                .Where(x => x.Id == Job.Id)
                .FirstOrDefault();
            if (JobDAO == null)
                return false;
            JobDAO.Id = Job.Id;
            JobDAO.CardId = Job.CardId;
            JobDAO.Name = Job.Name;
            JobDAO.Description = Job.Description;
            JobDAO.Order = Job.Order;
            JobDAO.StartAt = Job.StartAt;
            JobDAO.EndAt = Job.EndAt;
            JobDAO.Color = Job.Color;
            JobDAO.NoTodoDone = Job.NoTodoDone;
            JobDAO.CreatorId = Job.CreatorId;
            JobDAO.IsAllDay = Job.IsAllDay;
            JobDAO.UpdateAt = DateTime.Now;
            await DataContext.SaveChangesAsync();
            await SaveReference(Job);
            return true;
        }

        public async Task<bool> Delete(Job Job)
        {
            JobDAO? JobDAO = DataContext.Jobs
                .Where(x => x.Id == Job.Id)
                .FirstOrDefault();
            if (JobDAO == null)
                return false;
            JobDAO.DeleteAt = DateTime.Now;
            await DataContext.SaveChangesAsync();
            await SaveReference(Job);
            return true;
        }

        public async Task<bool> BulkDelete(List<Job> Jobs)
        {
            List<long> Ids = Jobs.Select(x => x.Id).ToList();
            await DataContext.Jobs.Where(x => Ids.Contains(x.Id))
                .UpdateFromQueryAsync(x => new JobDAO
                {
                    DeleteAt = DateTime.Now,
                });

            List<long> TodoIds = Jobs.SelectMany(x => x.Todos.Select(y => y.Id)).ToList();
            List<long> AppUserJobMappingIds = Jobs.SelectMany(x => x.AppUserJobMappings.Select(y => y.Id)).ToList();

            await DataContext.Todos.Where(x => TodoIds.Contains(x.Id)).DeleteFromQueryAsync();
            await DataContext.AppUserJobMappings.Where(x => AppUserJobMappingIds.Contains(x.Id)).DeleteFromQueryAsync();
            await DataContext.Jobs.Where(x => Ids.Contains(x.Id)).DeleteFromQueryAsync();
            return true;
        }

        private async Task SaveReference(Job Job)
        {
            if (Job.Todos == null || Job.Todos.Count == 0)
                await DataContext.Todos
                    .Where(x => x.JobId == Job.Id)
                    .DeleteFromQueryAsync();
            else
            {
                var TodoIds = Job.Todos.Select(x => x.Id).Distinct().ToList();
                await DataContext.Todos
                .Where(x => x.JobId == Job.Id)
                .DeleteFromQueryAsync();

                List<TodoDAO> TodoDAOs = new List<TodoDAO>();
                foreach (Todo Todo in Job.Todos)
                {
                    TodoDAO TodoDAO = new TodoDAO();
                    TodoDAO.Description = Todo.Description;
                    TodoDAO.JobId = Todo.JobId;
                    TodoDAO.IsDone = Todo.IsDone;
                    TodoDAOs.Add(TodoDAO);
                }
                await DataContext.Todos.AddRangeAsync(TodoDAOs);
                await DataContext.SaveChangesAsync();
            }

            if (Job.AppUserJobMappings == null || Job.AppUserJobMappings.Count == 0)
                await DataContext.AppUserJobMappings
                    .Where(x => x.JobId == Job.Id)
                    .DeleteFromQueryAsync();
            else
            {
                var AppUserJobMappingIds = Job.AppUserJobMappings.Select(x => x.Id).Distinct().ToList();
                await DataContext.AppUserJobMappings
                .Where(x => x.JobId == Job.Id)
                .DeleteFromQueryAsync();

                List<AppUserJobMappingDAO> AppUserJobMappingDAOs = new List<AppUserJobMappingDAO>();
                foreach (AppUserJobMapping AppUserJobMapping in Job.AppUserJobMappings)
                {
                    AppUserJobMappingDAO AppUserJobMappingDAO = new AppUserJobMappingDAO();
                    AppUserJobMappingDAO.AppUserId = AppUserJobMapping.AppUserId;
                    AppUserJobMappingDAO.JobId = AppUserJobMapping.JobId;
                    AppUserJobMappingDAOs.Add(AppUserJobMappingDAO);
                }
                await DataContext.AppUserJobMappings.AddRangeAsync(AppUserJobMappingDAOs);
                await DataContext.SaveChangesAsync();
            }
        }

        public async Task<bool> BulkMerge(List<Job> Jobs)
        {
            await DataContext.Todos.Where(x => Jobs.Select(x => x.Id).ToList().Contains(x.JobId.Value)).DeleteFromQueryAsync();
            foreach (Job Job in Jobs)
            {
                JobDAO JobDAO = new JobDAO();
                JobDAO.Id = Job.Id;
                JobDAO.CardId = Job.CardId;
                JobDAO.Name = Job.Name;
                JobDAO.Description = Job.Description;
                JobDAO.Order = Job.Order;
                JobDAO.StartAt = Job.StartAt;
                JobDAO.EndAt = Job.EndAt;
                JobDAO.Color = Job.Color;
                JobDAO.NoTodoDone = Job.NoTodoDone;
                JobDAO.IsAllDay = Job.IsAllDay;
                JobDAO.CreatorId = Job.CreatorId;
                JobDAO.CreatedAt = DateTime.Now;
                JobDAO.UpdateAt = DateTime.Now;
                if (JobDAO.Id == 0)
                    DataContext.Jobs.Add(JobDAO);
                else 
                    DataContext.Jobs.Update(JobDAO);
            }
            await DataContext.SaveChangesAsync();
            return true;
        }
    }
}
