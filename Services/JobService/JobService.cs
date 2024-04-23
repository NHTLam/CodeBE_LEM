using CodeBE_LEM.Entities;
using CodeBE_LEM.Repositories;
using CodeBE_LEM.Services.PermissionService;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CodeBE_LEM.Services.JobService
{
    public interface IJobService
    {
        Task<List<Job>> List();
        Task<List<Job>> ListOwn();
        Task<Job> Get(long Id);
        Task<Job> Create(Job Job);
        Task<Job> Update(Job Job);
        Task<Job> Delete(Job Job);
    }
    public class JobService : IJobService
    {
        private IUOW UOW;
        private IJobValidator JobValidator;
        private IPermissionService PermissionService;
        public JobService(
            IUOW UOW,
            IJobValidator JobValidator,
            IPermissionService PermissionService
        )
        {
            this.UOW = UOW;
            this.JobValidator = JobValidator;
            this.PermissionService = PermissionService;
        }
        public async Task<Job> Create(Job Job)
        {
            if (!await JobValidator.Create(Job))
                return Job;

            try
            {
                Job = CalcPercentTodoDone(Job);
                Job.CreatorId = PermissionService.GetAppUserId();
                if (Job.StartAt != null)
                {
                    Job.IsAllDay = Job.StartAt.Value.Day == Job.EndAt.Value.Day ? false : true;
                }
                await UOW.JobRepository.Create(Job);
                Job = await UOW.JobRepository.Get(Job.Id);
                return Job;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<Job> Delete(Job Job)
        {
            if (!await JobValidator.Delete(Job))
                return Job;

            try
            {
                Job = await Get(Job.Id);
                await UOW.JobRepository.Delete(Job);
                return Job;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<Job> Get(long Id)
        {
            Job Job = await UOW.JobRepository.Get(Id);
            if (Job == null)
                return null;
            await JobValidator.Get(Job);
            return Job;
        }

        public async Task<List<Job>> ListByCardIds(List<long> CardIds)
        {
            List<Job> Jobs = await UOW.JobRepository.ListByCardIds(CardIds);
            if (Jobs == null)
                return null;
            return Jobs;
        }

        public async Task<List<Job>> List()
        {
            try
            {
                List<Job> Jobs = await UOW.JobRepository.List();
                return Jobs;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public async Task<List<Job>> ListOwn()
        {
            try
            {
                List<long> JobIds = await UOW.JobRepository.ListJobIdByUserId(PermissionService.GetAppUserId());
                List<Job> Jobs = await UOW.JobRepository.List(JobIds);
                return Jobs;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public async Task<Job> Update(Job Job)
        {
            if (!await JobValidator.Update(Job))
                return Job;
            try
            {
                var oldData = await UOW.JobRepository.Get(Job.Id);
                Job = CalcPercentTodoDone(Job);
                if (Job.StartAt != null)
                {
                    Job.IsAllDay = Job.StartAt.Value.Day == Job.EndAt.Value.Day ? false : true;
                }
                Job.CreatorId = PermissionService.GetAppUserId();
                await UOW.JobRepository.Update(Job);
                Job = await UOW.JobRepository.Get(Job.Id);
                return Job;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        private Job CalcPercentTodoDone(Job Job)
        {
            if (Job.Todos != null && Job.Todos.Count > 0)
            {
                int countTodoDone = 0;
                foreach (var todo in Job.Todos)
                {
                    if (todo.IsDone == true)
                    {
                        countTodoDone++;
                    }
                } 
                decimal PercentTodoDone = (countTodoDone * 1.0M / Job.Todos.Count) * 100;
                Job.NoTodoDone = (int)PercentTodoDone;
            }
            return Job;
        }
    }
}
