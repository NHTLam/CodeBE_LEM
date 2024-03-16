using CodeBE_LEM.Entities;
using CodeBE_LEM.Models;
using CodeBE_LEM.Repositories;

namespace CodeBE_LEM.Services.AppUserS
{
    public interface IAppUserValidator
    {
        Task Get(AppUser AppUser);
        Task<bool> Create(AppUser AppUser);
        Task<bool> Update(AppUser AppUser);
        Task<bool> Delete(AppUser AppUser);
    }
    public class AppUserValidator : IAppUserValidator
    {
        private IUOW UOW;

        public AppUserValidator(IUOW UOW)
        {
            this.UOW = UOW;
        }

        public async Task Get(AppUser AppUser)
        {
        }

        public async Task<bool> Create(AppUser AppUser)
        {
            return true;
        }

        public async Task<bool> Update(AppUser AppUser)
        {
            return true;
        }

        public async Task<bool> Delete(AppUser AppUser)
        {
            return true;
        }
    }
}
