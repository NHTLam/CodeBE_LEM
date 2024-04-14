using CodeBE_LEM.Entities;
using CodeBE_LEM.Enums;
using CodeBE_LEM.Repositories;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CodeBE_LEM.Services.BoardService
{
    public interface IBoardService
    {
        Task<List<Board>> List();
        Task<Board> Get(long Id);
        Task<Board> GetOwn(long UserId);
        Task<Board> Create(Board Board);
        Task<Board> Update(Board Board);
        Task<Board> Delete(Board Board);
    }
    public class BoardService : IBoardService
    {
        private IUOW UOW;
        private IBoardValidator BoardValidator;
        public BoardService(
            IUOW UOW,
            IBoardValidator BoardValidator
        )
        {
            this.UOW = UOW;
            this.BoardValidator = BoardValidator;
        }
        public async Task<Board> Create(Board Board)
        {
            if (!await BoardValidator.Create(Board))
                return Board;

            try
            {
                if (Board.ImageUrl == null) Board.ImageUrl = "";
                Board.Code = string.Empty;
                await UOW.BoardRepository.Create(Board);
                await BuildCode(Board);
                Board = await UOW.BoardRepository.Get(Board.Id);
                return Board;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<Board> Delete(Board Board)
        {
            if (!await BoardValidator.Delete(Board))
                return Board;

            try
            {
                Board = await Get(Board.Id);
                await UOW.BoardRepository.Delete(Board);
                return Board;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<Board> Get(long Id)
        {
            Board Board = await UOW.BoardRepository.Get(Id);
            await AddJobDataForCards(Board);
            if (Board == null)
                return null;
            await BoardValidator.Get(Board);
            return Board;
        }

        public async Task<Board> GetOwn(long UserId)
        {
            List<long> BoardIds = (await UOW.BoardRepository.ListAppUserBoardMappingByAppUser(UserId)).
                Where(x => x.AppUserTypeId == AppUserTypeEnum.OWN.Id).
                Select(x => x.BoardId).ToList();
            Board Board = await UOW.BoardRepository.Get(BoardIds.FirstOrDefault());
            await AddJobDataForCards(Board);
            if (Board == null)
                return null;
            await BoardValidator.Get(Board);
            return Board;
        }

        private async Task AddJobDataForCards(Board Board)
        {
            if (Board.Cards != null || Board.Cards.Count > 0)
            {
                List<long> CardIds = Board.Cards.Select(x => x.Id).ToList();
                List<Job> Jobs = await UOW.JobRepository.ListByCardIds(CardIds);
                foreach (var Card in Board.Cards)
                {
                    Card.Jobs = Jobs.Where(x => x.CardId == Card.Id).ToList();
                }
            }
        }

        public async Task<List<Board>> List()
        {
            try
            {
                List<Board> Boards = await UOW.BoardRepository.List();
                return Boards;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public async Task<Board> Update(Board Board)
        {
            if (!await BoardValidator.Update(Board))
                return Board;
            try
            {
                var oldData = await UOW.BoardRepository.Get(Board.Id);
                await UOW.BoardRepository.Update(Board);
                await BuildCode(Board);
                Board = await UOW.BoardRepository.Get(Board.Id);
                return Board;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        private async Task BuildCode(Board Board)
        {
            Board.Code = "B" + Board.Id;
            await UOW.BoardRepository.UpdateCode(Board);
        }
    }
}
