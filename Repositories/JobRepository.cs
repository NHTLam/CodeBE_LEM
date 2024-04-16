﻿using CodeBE_LEM.Entities;
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
        Task<List<Job>> ListByCardIds(List<long> CardIds);
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
                    CompletePercent = x.CompletePercent,
                }).ToListAsync();

            foreach (Job Job in Jobs)
            {
                Job.Todos = Todos
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
            }).ToListAsync();

            var TodoQuery = DataContext.Todos.AsNoTracking();
            List<Todo> Todos = await TodoQuery
                .Select(x => new Todo
                {
                    Id = x.Id,
                    Description = x.Description,
                    JobId = x.JobId,
                    CompletePercent = x.CompletePercent,
                }).ToListAsync();

            foreach (Job Job in Jobs)
            {
                Job.Todos = Todos
                    .Where(x => x.JobId == Job.Id)
                    .ToList();
            }

            return Jobs;
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
                    CompletePercent = x.CompletePercent,
                }).ToListAsync();

            foreach (Job Job in Jobs)
            {
                Job.Todos = Todos
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
                    CompletePercent = x.CompletePercent,
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
                .Where(x => !TodoIds.Contains(x.Id))
                .DeleteFromQueryAsync();

                List<TodoDAO> TodoDAOs = new List<TodoDAO>();
                foreach (Todo Todo in Job.Todos)
                {
                    TodoDAO TodoDAO = new TodoDAO();
                    TodoDAO.Description = Todo.Description;
                    TodoDAO.JobId = Todo.JobId;
                    TodoDAO.CompletePercent = Todo.CompletePercent;
                    TodoDAOs.Add(TodoDAO);
                }
                await DataContext.Todos.AddRangeAsync(TodoDAOs);
                await DataContext.SaveChangesAsync();
            }
        }

        public async Task<bool> BulkMerge(List<Job> Jobs)
        {
            List<JobDAO> JobDAOs = new List<JobDAO>();
            foreach (Job Job in Jobs)
            {
                JobDAO JobDAO = new JobDAO();
                JobDAO.Id = Job.Id;
                JobDAO.Name = Job.Name;
                JobDAO.CardId = Job.CardId;
                JobDAO.Description = Job.Description;
                JobDAO.Order = Job.Order;
                JobDAO.StartAt = Job.StartAt;
                JobDAO.EndAt = Job.EndAt;
                JobDAO.Color = Job.Color;
                JobDAO.NoTodoDone = Job.NoTodoDone;
                JobDAO.IsAllDay = Job.IsAllDay;
                JobDAOs.Add(JobDAO);
            }
            await DataContext.BulkMergeAsync(JobDAOs);
            return true;
        }
    }
}
