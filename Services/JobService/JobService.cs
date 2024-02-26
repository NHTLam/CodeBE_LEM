using CodeBE_LEM.Entities;
using CodeBE_LEM.Repositories;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CodeBE_LEM.Services.JobService
{
    public interface IJobService
    {
        Task<List<Job>> List();
        Task<Job> Get(long Id);
        Task<Job> Create(Job Job);
        Task<Job> Update(Job Job);
        Task<Job> Delete(Job Job);
    }
    public class JobService : IJobService
    {
        private IUOW UOW;
        private IJobValidator JobValidator;
        public JobService(
            IUOW UOW,
            IJobValidator JobValidator
        )
        {
            this.UOW = UOW;
            this.JobValidator = JobValidator;
        }
        public async Task<Job> Create(Job Job)
        {
            if (!await JobValidator.Create(Job))
                return Job;

            try
            {
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

        public async Task<Job> Update(Job Job)
        {
            if (!await JobValidator.Update(Job))
                return Job;
            try
            {
                var oldData = await UOW.JobRepository.Get(Job.Id);
                await UOW.JobRepository.Update(Job);
                Job = await UOW.JobRepository.Get(Job.Id);
                return Job;
            }
            catch (Exception ex)
            {
            }
            return null;
        }
    }
}
