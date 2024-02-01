using HikeBE.Entities;
using HikeBE.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace HikeBE.Services.SHike
{
    public interface IHikeValidator
    {
        Task Get(Hike Hike);
        Task<bool> Create(Hike Hike);
        Task<bool> Update(Hike Hike);
        Task<bool> Delete(Hike Hike);
    }
    public class HikeValidator : IHikeValidator
    {
        private IUOW UOW;

        public HikeValidator(IUOW UOW)
        {
            this.UOW = UOW;
        }

        public async Task Get(Hike Hike)
        {
        }

        public async Task<bool> Create(Hike Hike)
        {
            return true;
        }

        public async Task<bool> Update(Hike Hike)
        {
            return true;
        }

        public async Task<bool> Delete(Hike Hike)
        {
            return true;
        }
    }
}
