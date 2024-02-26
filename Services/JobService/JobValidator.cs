using CodeBE_LEM.Entities;
using CodeBE_LEM.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CodeBE_LEM.Services.JobService
{
    public interface IJobValidator
    {
        Task Get(Job Job);
        Task<bool> Create(Job Job);
        Task<bool> Update(Job Job);
        Task<bool> Delete(Job Job);
    }
    public class JobValidator : IJobValidator
    {
        private IUOW UOW;

        public JobValidator(IUOW UOW)
        {
            this.UOW = UOW;
        }

        public async Task Get(Job Job)
        {
        }

        public async Task<bool> Create(Job Job)
        {
            return true;
        }

        public async Task<bool> Update(Job Job)
        {
            return true;
        }

        public async Task<bool> Delete(Job Job)
        {
            return true;
        }
    }
}
