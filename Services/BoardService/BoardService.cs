﻿using CodeBE_LEM.Entities;
using CodeBE_LEM.Enums;
using CodeBE_LEM.Repositories;
using CodeBE_LEM.Services.PermissionService;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Linq;

namespace CodeBE_LEM.Services.BoardService
{
    public interface IBoardService
    {
        Task<List<Board>> List();
        Task<List<Board>> ListByClassroom(long ClassroomId);
        Task<Board> Get(long Id);
        Task<Board> GetOwn(long UserId);
        Task<Board> Create(Board Board);
        Task<Board> Update(Board Board);
        Task<Board> Delete(Board Board);
        Task<List<Board>> CreateBoardsForClass(CreateBoardsFunction CreateBoardsFunction);
        Task<List<Card>> ListCardByUserId();
        Task<bool> DuplicateCard(Card Card);
        Task<bool> DeleteCard(Card Card);

    }
    public class BoardService : IBoardService
    {
        private IUOW UOW;
        private IBoardValidator BoardValidator;
        private IPermissionService PermissionService;
        public BoardService(
            IUOW UOW,
            IBoardValidator BoardValidator,
            IPermissionService PermissionService
        )
        {
            this.UOW = UOW;
            this.BoardValidator = BoardValidator;
            this.PermissionService = PermissionService;
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

        public async Task<bool> DuplicateCard(Card Card)
        {
            try
            {
                Board Board = await UOW.BoardRepository.Get(Card.BoardId);
                var oldData = Board;
                Card.Id = 0;
                Board.Cards.Add(Card);
                Board = await Update(Board);
                var newCardId = Board.Cards.Where(x => !oldData.Cards.Select(x => x.Id).Contains(x.Id)).FirstOrDefault()!.Id;
                foreach (var Job in Card.Jobs)
                {
                    Job.Id = 0;
                    Job.CardId = newCardId;
                }
                await UOW.JobRepository.BulkMerge(Card.Jobs);
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        public async Task<bool> DeleteCard(Card Card)
        {
            try
            {
                Board Board = await UOW.BoardRepository.Get(Card.BoardId);
                List<Job> Jobs = await UOW.JobRepository.List(Card.Jobs.Select(x => x.Id).ToList());
                await UOW.JobRepository.BulkDelete(Jobs);
                await UOW.BoardRepository.DeleteCard(Card);
                return true;
            }
            catch (Exception ex)
            {

            }
            return false;
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
            try
            {
                Board Board = await UOW.BoardRepository.Get(Id);
                await AddJobDataForCardsForBoard(Board);
                if (Board == null)
                    return null;
                await BoardValidator.Get(Board);
                return Board;
            }
            catch(Exception ex) 
            {
            }
            return null;
        }

        public async Task<Board> GetOwn(long UserId)
        {
            try
            {

                List<long> BoardIds = (await UOW.BoardRepository.ListAppUserBoardMappingByAppUser(UserId)).
                    Where(x => x.AppUserTypeId == AppUserTypeEnum.OWN.Id).
                    Select(x => x.BoardId).ToList();
                Board Board = await UOW.BoardRepository.Get(BoardIds.FirstOrDefault());
                await AddJobDataForCardsForBoard(Board);
                if (Board == null)
                    return null;
                await BoardValidator.Get(Board);
                return Board;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        private async Task AddJobDataForCardsForBoard(Board Board)
        {
            if (Board == null) return;
            if (Board.Cards != null && Board.Cards.Count > 0)
            {
                List<long> CardIds = Board.Cards.Select(x => x.Id).ToList();
                List<Job> Jobs = await UOW.JobRepository.ListByCardIds(CardIds);
                foreach (var Card in Board.Cards)
                {
                    Card.Jobs = Jobs.Where(x => x.CardId == Card.Id).ToList();
                }
            }
        }

        private async Task AddJobDataForCardsForBoards(List<Board> Boards)
        {
            var ValidCardIds = Boards.Where(x => x.Cards != null || x.Cards.Count > 0).SelectMany(x => x.Cards.Select(x => x.Id)).Distinct().ToList();
            List<Job> Jobs = await UOW.JobRepository.ListByCardIds(ValidCardIds);
            foreach (var Board in Boards)
            {
                if (Board.Cards != null && Board.Cards.Count > 0)
                {
                    foreach (var Card in Board.Cards)
                    {
                        Card.Jobs = Jobs.Where(x => x.CardId == Card.Id).ToList();
                    }
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

        public async Task<List<Card>> ListCardByUserId()
        {
            try
            {
                var Board = await GetOwn(PermissionService.GetAppUserId());
                List<Card> Cards = Board.Cards;
                return Cards;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public async Task<List<Board>> ListByClassroom(long ClassroomId)
        {
            try
            {
                List<Board> Boards = await UOW.BoardRepository.ListByClassroom(ClassroomId);
                if (Boards == null || Boards.Count == 0)
                {
                    return new List<Board>();
                }
                await AddJobDataForCardsForBoards(Boards);
                return Boards;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public async Task<List<Board>> CreateBoardsForClass(CreateBoardsFunction CreateBoardsFunction)
        {
            try
            {
                List<Board> Boards = new List<Board>();
                List<Board> OldBoard = await ListByClassroom(CreateBoardsFunction.ClassroomId);
                List<long> StudentIdAlreadyHaveGroups = OldBoard.SelectMany(x => x.AppUserBoardMappings.Select(y => y.AppUserId)).Distinct().ToList();
                List<AppUser> ClassroomAppUsers = await UOW.AppUserRepository.List();
                ClassroomAppUsers = ClassroomAppUsers.Where(x => x.AppUserClassroomMappings?.Select(y => y.ClassroomId)?.Contains(CreateBoardsFunction.ClassroomId) ?? false).ToList();
                var Teachers = ClassroomAppUsers.Where(x => x.AppUserClassroomMappings?.Select(y => y.Role.Name)?.Contains("Teacher") ?? false).ToList();

                int newNumberOfGroups = CreateBoardsFunction.NumberOfGroups - OldBoard.Count;
                List<AppUser> Students = ClassroomAppUsers.Where(x => CreateBoardsFunction.AppUserIds.Contains(x.Id)).Where(x => !StudentIdAlreadyHaveGroups.Contains(x.Id)).ToList();

                if (newNumberOfGroups > 0)
                {
                    int numberOfStudentInOneGroup = (int)Math.Round(Students.Count * 1.0M / newNumberOfGroups, 0);

                    for (int i = 0; i < newNumberOfGroups; i++)
                    {
                        var appUserForAppUserBoardMappings = Students.Take(numberOfStudentInOneGroup).ToList();
                        appUserForAppUserBoardMappings.AddRange(Teachers);

                        var newAppUsers = Students.Where(x => !appUserForAppUserBoardMappings.Select(x => x.Id).Contains(x.Id)).ToList();
                        if (newAppUsers.Count < numberOfStudentInOneGroup)
                        {
                            newAppUsers = Students;
                            appUserForAppUserBoardMappings = Students;
                            appUserForAppUserBoardMappings.AddRange(Teachers);
                        }

                        List<AppUserBoardMapping> AppUserBoardMappings = new List<AppUserBoardMapping>();
                        foreach (var appUser in appUserForAppUserBoardMappings)
                        {
                            AppUserBoardMapping AppUserBoardMapping = new AppUserBoardMapping();
                            AppUserBoardMapping.AppUserId = appUser.Id;
                            if (Teachers.Select(x => x.Id).Contains(appUser.Id))
                                AppUserBoardMapping.AppUserTypeId = AppUserTypeEnum.OWN.Id;
                            else
                                AppUserBoardMapping.AppUserTypeId = AppUserTypeEnum.COOPERATOR.Id;
                            AppUserBoardMappings.Add(AppUserBoardMapping);
                        }

                        Board Board = new Board();
                        Board.Code = "";
                        Board.Name = $"Group {i + 1}";
                        Board.Description = "";
                        Board.ImageUrl = "";
                        Board.ClassroomId = CreateBoardsFunction.ClassroomId;
                        Board.AppUserBoardMappings = AppUserBoardMappings;
                        Boards.Add(Board);

                        Students = newAppUsers;
                    }
                }

                var oldData = Boards;
                var Ids = await UOW.BoardRepository.BulkMerge(Boards);
                Boards = (await ListByClassroom(CreateBoardsFunction.ClassroomId)).Where(x => Ids.Contains(x.Id)).ToList();
                await BuildCodes(Boards);
                await BulkMergeAppUserBoardMapping(Boards, oldData);

                return Boards;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        private async Task BulkMergeAppUserBoardMapping(List<Board> Boards, List<Board> oldData)
        {
            try
            {
                List<AppUserBoardMapping> BulkMergeAppUserBoardMappings = new List<AppUserBoardMapping>();
                foreach (var board in oldData)
                {
                    board.Id = Boards.Where(x => x.Name == board.Name).FirstOrDefault().Id;
                    foreach (var BulkMergeAppUserBoardMapping in board.AppUserBoardMappings)
                    {
                        BulkMergeAppUserBoardMapping.BoardId = board.Id;
                        BulkMergeAppUserBoardMappings.Add(BulkMergeAppUserBoardMapping);
                    }
                }
                await UOW.BoardRepository.BulkMergeAppUserBoardMapping(BulkMergeAppUserBoardMappings);
            }
            catch (Exception ex)
            {

            }
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

        private async Task BuildCodes(List<Board> Boards)
        {
            foreach (var Board in Boards)
            {
                Board.Code = "B" + Board.Id;
            }
            await UOW.BoardRepository.BulkUpdateCode(Boards);
        }
    }
}
