using CodeBE_LEM.Entities;
using CodeBE_LEM.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace CodeBE_LEM.Repositories
{
    public interface IBoardRepository
    {
        Task<List<Board>> List();
        Task<List<Board>> ListByClassroom(long ClassroomId);
        Task<Board> Get(long Id);
        Task<bool> Create(Board Board);
        Task<bool> Update(Board Board);
        Task<bool> Delete(Board Board);
        Task<bool> DeleteCard(Card Card);
        Task<bool> UpdateCode(Board Board);
        Task<List<long>> BulkMerge(List<Board> Boards);
        Task<bool> BulkUpdateCode(List<Board> Boards);
        Task<bool> BulkMergeAppUserBoardMapping(List<AppUserBoardMapping> AppUserBoardMappings);
        Task<List<AppUserBoardMapping>> ListAppUserBoardMappingByAppUser(long AppUserId);
    }

    public class BoardRepository : IBoardRepository
    {
        private DataContext DataContext;

        public BoardRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        public async Task<List<Board>> List()
        {
            IQueryable<BoardDAO> query = DataContext.Boards.AsNoTracking();
            List<Board> Boards = await query.AsNoTracking()
            .Where(x => x.DeletedAt == null)
            .Select(x => new Board
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                Description = x.Description,
                IsFavourite = x.IsFavourite,
                ImageUrl = x.ImageUrl,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                ClassroomId = x.ClassroomId,
            }).ToListAsync();

            var CardQuery = DataContext.Cards.AsNoTracking();
            List<Card> Cards = await CardQuery
                .Where(x => x.DeletedAt == null)
                .Select(x => new Card
                {
                    Id = x.Id,
                    Name = x.Name,
                    BoardId = x.BoardId,
                    Order = x.Order,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    DeletedAt = x.DeletedAt,
                }).ToListAsync();

            var AppUserBoardMappingQuery = DataContext.AppUserBoardMappings.AsNoTracking();
            List<AppUserBoardMapping> AppUserBoardMappings = await AppUserBoardMappingQuery
                .Select(x => new AppUserBoardMapping
                {
                    Id = x.Id,
                    AppUserId = x.AppUserId,
                    BoardId = x.BoardId,
                    AppUserTypeId = x.AppUserTypeId,
                }).ToListAsync();

            foreach (Board Board in Boards)
            {
                Board.Cards = Cards
                    .Where(x => x.BoardId == Board.Id)
                    .ToList();
                Board.AppUserBoardMappings = AppUserBoardMappings
                    .Where(x => x.BoardId == Board.Id)
                    .ToList();
            }

            return Boards;
        }

        public async Task<List<Board>> ListByClassroom(long ClassroomId)
        {
            IQueryable<BoardDAO> query = DataContext.Boards.AsNoTracking();
            List<Board> Boards = await query.AsNoTracking()
            .Where(x => x.DeletedAt == null)
            .Where(x => x.ClassroomId == ClassroomId)
            .Select(x => new Board
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                Description = x.Description,
                IsFavourite = x.IsFavourite,
                ImageUrl = x.ImageUrl,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                ClassroomId = x.ClassroomId,
            }).ToListAsync();

            var CardQuery = DataContext.Cards.AsNoTracking();
            List<Card> Cards = await CardQuery
                .Where(x => x.DeletedAt == null)
                .Select(x => new Card
                {
                    Id = x.Id,
                    Name = x.Name,
                    BoardId = x.BoardId,
                    Order = x.Order,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    DeletedAt = x.DeletedAt,
                }).ToListAsync();

            var AppUserBoardMappingQuery = DataContext.AppUserBoardMappings.AsNoTracking();
            List<AppUserBoardMapping> AppUserBoardMappings = await AppUserBoardMappingQuery
                .Select(x => new AppUserBoardMapping
                {
                    Id = x.Id,
                    AppUserId = x.AppUserId,
                    BoardId = x.BoardId,
                    AppUserTypeId = x.AppUserTypeId,
                }).ToListAsync();

            foreach (Board Board in Boards)
            {
                Board.Cards = Cards
                    .Where(x => x.BoardId == Board.Id)
                    .ToList();
                Board.AppUserBoardMappings = AppUserBoardMappings
                    .Where(x => x.BoardId == Board.Id)
                    .ToList();
            }

            return Boards;
        }

        public async Task<List<AppUserBoardMapping>> ListAppUserBoardMappingByAppUser(long AppUserId)
        {
            IQueryable<AppUserBoardMappingDAO> query = DataContext.AppUserBoardMappings.AsNoTracking();
            List<AppUserBoardMapping> AppUserBoardMappings = await query.AsNoTracking()
            .Where(x => x.AppUserId == AppUserId)
            .Select(x => new AppUserBoardMapping
            {
                Id = x.Id,
                AppUserId = x.AppUserId,
                BoardId = x.BoardId,
                AppUserTypeId = x.AppUserTypeId,
            }).ToListAsync();

            return AppUserBoardMappings;
        }

        public async Task<Board> Get(long Id)
        {
            Board? Board = await DataContext.Boards.AsNoTracking()
            .Where(x => x.DeletedAt == null)
            .Where(x => x.Id == Id)
            .Select(x => new Board()
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                Description = x.Description,
                IsFavourite = x.IsFavourite,
                ImageUrl = x.ImageUrl,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                ClassroomId = x.ClassroomId,
            }).FirstOrDefaultAsync();

            if (Board == null)
                return null;
            Board.Cards = await DataContext.Cards.AsNoTracking()
                .Where(x => x.BoardId == Board.Id)
                .Select(x => new Card
                {
                    Id = x.Id,
                    Name = x.Name,
                    BoardId = x.BoardId,
                    Order = x.Order,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    DeletedAt = x.DeletedAt,
                }).ToListAsync();

            Board.AppUserBoardMappings = await DataContext.AppUserBoardMappings.AsNoTracking()
                .Where(x => x.BoardId == Board.Id)
                .Select(x => new AppUserBoardMapping
                {
                    Id = x.Id,
                    AppUserId = x.AppUserId,
                    BoardId = x.BoardId,
                    AppUserTypeId = x.AppUserTypeId,
                    AppUser = x.AppUser == null ? null : new AppUser
                    {
                        Id = x.AppUser.Id,
                        UserName = x.AppUser.UserName,
                        FullName = x.AppUser.FullName,
                    }
                }).ToListAsync();

            return Board;
        }

        public async Task<bool> Create(Board Board)
        {
            BoardDAO BoardDAO = new BoardDAO();
            BoardDAO.Name = Board.Name;
            BoardDAO.Code = Board.Code;
            BoardDAO.Description = Board.Description;
            BoardDAO.IsFavourite = Board.IsFavourite;
            BoardDAO.ImageUrl = Board.ImageUrl;
            BoardDAO.CreatedAt = DateTime.Now;
            BoardDAO.UpdatedAt = DateTime.Now;
            BoardDAO.ClassroomId = Board.ClassroomId;
            DataContext.Boards.Add(BoardDAO);
            await DataContext.SaveChangesAsync();
            Board.Id = BoardDAO.Id;
            await SaveReference(Board);
            return true;
        }

        public async Task<bool> Update(Board Board)
        {
            BoardDAO? BoardDAO = DataContext.Boards
                .Where(x => x.Id == Board.Id)
                .FirstOrDefault();
            if (BoardDAO == null)
                return false;
            BoardDAO.Id = Board.Id;
            BoardDAO.Name = Board.Name;
            BoardDAO.Code = Board.Code;
            BoardDAO.Description = Board.Description;
            BoardDAO.ImageUrl = Board.ImageUrl;
            BoardDAO.IsFavourite = Board.IsFavourite;
            BoardDAO.ClassroomId = Board.ClassroomId;
            BoardDAO.UpdatedAt = DateTime.Now;
            await DataContext.SaveChangesAsync();
            await SaveReference(Board);
            return true;
        }

        public async Task<bool> Delete(Board Board)
        {
            BoardDAO? BoardDAO = DataContext.Boards
                .Where(x => x.Id == Board.Id)
                .FirstOrDefault();
            if (BoardDAO == null)
                return false;
            BoardDAO.DeletedAt = DateTime.Now;
            await DataContext.SaveChangesAsync();
            await SaveReference(Board);
            return true;
        }

        public async Task<bool> DeleteCard(Card Card)
        {
            CardDAO? CardDAO = DataContext.Cards
                .Where(x => x.Id == Card.Id)
                .FirstOrDefault();
            if (CardDAO == null)
                return false;
            DataContext.Cards.Remove(CardDAO);
            await DataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateCode(Board Board)
        {
            BoardDAO? BoardDAO = DataContext.Boards
                .Where(x => x.Id == Board.Id)
                .FirstOrDefault();
            if (BoardDAO == null)
                return false;
            BoardDAO.Id = Board.Id;
            BoardDAO.Code = Board.Code;
            await DataContext.SaveChangesAsync();
            return true;
        }

        private async Task SaveReference(Board Board)
        {
            if (Board.Cards == null || Board.Cards.Count == 0)
                await DataContext.Cards
                    .Where(x => x.BoardId == Board.Id)
                    .DeleteFromQueryAsync();
            else
            {
                var CardIds = Board.Cards.Select(x => x.Id).Distinct().ToList();
                await DataContext.Cards
                .Where(x => x.BoardId == Board.Id)
                .Where(x => !CardIds.Contains(x.Id))
                .DeleteFromQueryAsync();

                List<CardDAO> CardDAOUpdates = new List<CardDAO>();
                List<CardDAO> CardDAOCreates = new List<CardDAO>();
                foreach (Card Card in Board.Cards)
                {
                    CardDAO CardDAO = new CardDAO();
                    CardDAO.Id = Card.Id;
                    CardDAO.BoardId = Board.Id;
                    CardDAO.Name = Card.Name;
                    CardDAO.Order = Card.Order;
                    CardDAO.CreatedAt = DateTime.Now;
                    CardDAO.UpdatedAt = DateTime.Now;
                    if (CardDAO.Id == 0)
                        CardDAOCreates.Add(CardDAO);
                    else
                        CardDAOUpdates.Add(CardDAO);
                }

                foreach (var CardDAOUpdate in CardDAOUpdates)
                {
                    DataContext.Cards.Update(CardDAOUpdate);
                }

                foreach (var CardDAOCreate in CardDAOCreates)
                {
                    DataContext.Cards.Add(CardDAOCreate);
                }
            }

            if (Board.AppUserBoardMappings == null || Board.AppUserBoardMappings.Count == 0)
                await DataContext.AppUserBoardMappings
                    .Where(x => x.BoardId == Board.Id)
                    .DeleteFromQueryAsync();
            else
            {
                var AppUserBoardMappingIds = Board.AppUserBoardMappings.Select(x => x.Id).Distinct().ToList();
                await DataContext.AppUserBoardMappings
                .Where(x => x.BoardId == Board.Id)
                .Where(x => !AppUserBoardMappingIds.Contains(x.Id))
                .DeleteFromQueryAsync();

                List<AppUserBoardMappingDAO> AppUserBoardMappingDAOCreates = new List<AppUserBoardMappingDAO>();
                List<AppUserBoardMappingDAO> AppUserBoardMappingDAOUpdates = new List<AppUserBoardMappingDAO>();
                foreach (AppUserBoardMapping AppUserBoardMapping in Board.AppUserBoardMappings)
                {
                    AppUserBoardMappingDAO AppUserBoardMappingDAO = new AppUserBoardMappingDAO();
                    AppUserBoardMappingDAO.Id = AppUserBoardMapping.Id;
                    AppUserBoardMappingDAO.BoardId = Board.Id;
                    AppUserBoardMappingDAO.AppUserId = AppUserBoardMapping.AppUserId;
                    AppUserBoardMappingDAO.AppUserTypeId = AppUserBoardMapping.AppUserTypeId;
                    if (AppUserBoardMappingDAO.Id == 0)
                        AppUserBoardMappingDAOCreates.Add(AppUserBoardMappingDAO);
                    else
                        AppUserBoardMappingDAOUpdates.Add(AppUserBoardMappingDAO);
                }

                foreach (var AppUserBoardMappingDAOCreate in AppUserBoardMappingDAOCreates)
                {
                    DataContext.AppUserBoardMappings.Add(AppUserBoardMappingDAOCreate);
                }

                foreach (var AppUserBoardMappingDAOUpdate in AppUserBoardMappingDAOUpdates)
                {
                    DataContext.AppUserBoardMappings.Update(AppUserBoardMappingDAOUpdate);
                }
            }

            await DataContext.SaveChangesAsync();
        }

        public async Task<List<long>> BulkMerge(List<Board> Boards)
        {
            await DataContext.AppUserBoardMappings.Where(x => Boards.Select(x => x.Id).ToList().Contains(x.BoardId)).DeleteFromQueryAsync();
            List<BoardDAO> BoardDAOs = new List<BoardDAO>();
            List<Board> oldBoards = Boards;
            foreach (Board Board in Boards)
            {
                BoardDAO BoardDAO = new BoardDAO();
                BoardDAO.Id = Board.Id;
                BoardDAO.Name = Board.Name;
                BoardDAO.Code = Board.Code;
                BoardDAO.Description = Board.Description;
                BoardDAO.IsFavourite = Board.IsFavourite;
                BoardDAO.ImageUrl = Board.ImageUrl;
                BoardDAO.CreatedAt = DateTime.Now;
                BoardDAO.UpdatedAt = DateTime.Now;
                BoardDAO.ClassroomId = Board.ClassroomId;
                BoardDAOs.Add(BoardDAO);
                if (BoardDAO.Id == 0)
                    DataContext.Boards.Add(BoardDAO);
                else
                    DataContext.Boards.Update(BoardDAO);
            }
            await DataContext.SaveChangesAsync();

            var Ids = BoardDAOs.Select(x => x.Id).ToList();
            return Ids;
        }

        public async Task<bool> BulkUpdateCode(List<Board> Boards)
        {
            List<BoardDAO>? BoardDAOs = DataContext.Boards
                .Where(x => Boards.Select(y => y.Id).Contains(x.Id))
                .ToList();
            if (BoardDAOs == null)
                return false;
            foreach (BoardDAO BoardDAO in BoardDAOs)
            {
                string code = Boards.Where(x => x.Id == BoardDAO.Id).FirstOrDefault().Code;
                BoardDAO.Code = code;
                DataContext.Boards.Update(BoardDAO);
            }
            await DataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> BulkMergeAppUserBoardMapping(List<AppUserBoardMapping> AppUserBoardMappings)
        {
            List<AppUserBoardMappingDAO> AppUserBoardMappingDAOs = new List<AppUserBoardMappingDAO>();
            foreach (var appUserBoardMapping in AppUserBoardMappings)
            {
                AppUserBoardMappingDAO AppUserBoardMappingDAO = new AppUserBoardMappingDAO();
                AppUserBoardMappingDAO.AppUserId = appUserBoardMapping.AppUserId;
                AppUserBoardMappingDAO.BoardId = appUserBoardMapping.BoardId;
                AppUserBoardMappingDAO.AppUserTypeId = appUserBoardMapping.AppUserTypeId;
                AppUserBoardMappingDAOs.Add(AppUserBoardMappingDAO);
            }
            await DataContext.AppUserBoardMappings.AddRangeAsync(AppUserBoardMappingDAOs);
            await DataContext.SaveChangesAsync();

            return true;
        }

    }
}
