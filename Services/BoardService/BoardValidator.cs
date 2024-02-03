using CodeBE_LEM.Entities;
using CodeBE_LEM.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CodeBE_LEM.Services.BoardService
{
    public interface IBoardValidator
    {
        Task Get(Board Board);
        Task<bool> Create(Board Board);
        Task<bool> Update(Board Board);
        Task<bool> Delete(Board Board);
    }
    public class BoardValidator : IBoardValidator
    {
        private IUOW UOW;

        public BoardValidator(IUOW UOW)
        {
            this.UOW = UOW;
        }

        public async Task Get(Board Board)
        {
        }

        public async Task<bool> Create(Board Board)
        {
            return true;
        }

        public async Task<bool> Update(Board Board)
        {
            return true;
        }

        public async Task<bool> Delete(Board Board)
        {
            return true;
        }
    }
}
