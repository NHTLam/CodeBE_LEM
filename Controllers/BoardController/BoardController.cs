using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CodeBE_LEM.Services.BoardService;
using CodeBE_LEM.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using CodeBE_LEM.Controllers.AppUserController;
using CodeBE_LEM.Controllers.AttachmentController;
using CodeBE_LEM.Services.PermissionService;

namespace CodeBE_LEM.Controllers.BoardController
{
    [ApiController]
    public class BoardController : ControllerBase
    {
        private IBoardService BoardService;
        private IPermissionService PermissionService;

        public BoardController(
            IBoardService BoardService,
            IPermissionService PermissionService
        )
        {
            this.BoardService = BoardService;
            this.PermissionService = PermissionService;
        }

        [Route(BoardRoute.List), HttpPost, Authorize]
        public async Task<ActionResult<List<Board_BoardDTO>>> List()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<Board> Boards = await BoardService.List();
            List<Board_BoardDTO> Board_BoardDTOs = Boards
                .Select(c => new Board_BoardDTO(c)).ToList();

            return Board_BoardDTOs;
        }

        [Route(BoardRoute.ListCardByUserId), HttpPost, Authorize]
        public async Task<ActionResult<List<Board_CardDTO>>> ListCardByUserId()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<Card> Cards = await BoardService.ListCardByUserId();
            List<Board_CardDTO> Board_CardDTOs = Cards
                .Select(c => new Board_CardDTO(c)).ToList();

            return Board_CardDTOs;
        }

        [Route(BoardRoute.DuplicateCard), HttpPost, Authorize]
        public async Task<ActionResult<bool>> DuplicateCard([FromBody] Board_CardDTO Board_CardDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (Board_CardDTO.ClassroomId != null && Board_CardDTO.ClassroomId != 0)
            {
                if (!await PermissionService.HasPermission(BoardRoute.DuplicateCard, Board_CardDTO.ClassroomId.Value))
                {
                    return Forbid();
                }
            }
            else
            {
                Board_CardDTO.ClassroomId = null; // Gán lại để tránh lỗi FK
            }


            Card Card = ConvertCardDTOToEntity(Board_CardDTO);
            bool isSuccess = await BoardService.DuplicateCard(Card);
            
            if (isSuccess)
                return Ok(isSuccess);
            else 
                return BadRequest(isSuccess);
        }

        [Route(BoardRoute.DeleteCard), HttpPost, Authorize]
        public async Task<ActionResult<bool>> DeleteCard([FromBody] Board_CardDTO Board_CardDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Card Card = ConvertCardDTOToEntity(Board_CardDTO);
            bool isSuccess = await BoardService.DeleteCard(Card);

            if (isSuccess)
                return Ok(isSuccess);
            else
                return BadRequest(isSuccess);
        }

        [Route(BoardRoute.ListByClassroom), HttpPost, Authorize]
        public async Task<ActionResult<List<Board_BoardDTO>>> ListByClassroom([FromBody] Board_BoardDTO Board_BoardDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (Board_BoardDTO.ClassroomId == null)
                return BadRequest(ModelState);

            List<Board> Boards = await BoardService.ListByClassroom(Board_BoardDTO.ClassroomId.Value);
            List<Board_BoardDTO> Board_BoardDTOs = Boards
                .Select(c => new Board_BoardDTO(c)).ToList();

            return Board_BoardDTOs;
        }

        [Route(BoardRoute.CreateBoardsForClass), HttpPost, Authorize]
        public async Task<ActionResult<List<Board_BoardDTO>>?> CreateBoardsForClass([FromBody] Board_CreateBoardsFunctionDTO Board_CreateBoardsFunctionDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (Board_CreateBoardsFunctionDTO.ClassroomId != 0)
            {
                if (!await PermissionService.HasPermission(BoardRoute.CreateBoardsForClass, Board_CreateBoardsFunctionDTO.ClassroomId))
                {
                    return Forbid();
                }
            }

            CreateBoardsFunction CreateBoardsFunction = ConvertDTOToEntity(Board_CreateBoardsFunctionDTO);
            List<Board> Boards = await BoardService.CreateBoardsForClass(CreateBoardsFunction);
            List<Board_BoardDTO> Board_BoardDTOs = Boards
                .Select(c => new Board_BoardDTO(c)).ToList();

            return Board_BoardDTOs;
        }

        [Route(BoardRoute.Get), HttpPost, Authorize]
        public async Task<ActionResult<Board_BoardDTO>?> Get([FromBody] Board_BoardDTO Board_BoardDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Board Board = await BoardService.Get(Board_BoardDTO.Id);
            if (Board == null)
                return null;
            Board_BoardDTO = new Board_BoardDTO(Board);
            return Board_BoardDTO;
        }

        [Route(BoardRoute.GetOwn), HttpPost, Authorize]
        public async Task<ActionResult<Board_BoardDTO>?> GetOwn([FromBody] Board_AppUserBoardMappingDTO Board_AppUserBoardMappingDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Board Board = await BoardService.GetOwn(Board_AppUserBoardMappingDTO.AppUserId);
            if (Board == null)
                return null;
            Board_BoardDTO Board_BoardDTO = new Board_BoardDTO(Board);
            return Board_BoardDTO;
        }

        [Route(BoardRoute.Create), HttpPost, Authorize]
        public async Task<ActionResult<Board_BoardDTO>> Create([FromBody] Board_BoardDTO Board_BoardDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (Board_BoardDTO.ClassroomId != null && Board_BoardDTO.ClassroomId != 0)
            {
                if (!await PermissionService.HasPermission(BoardRoute.Create, Board_BoardDTO.ClassroomId.Value))
                {
                    return Forbid();
                }
            }
            else
            {
                Board_BoardDTO.ClassroomId = null; // Gán lại để tránh lỗi FK
            }

            Board Board = ConvertDTOToEntity(Board_BoardDTO);

            Board = await BoardService.Create(Board);
            Board_BoardDTO = new Board_BoardDTO(Board);
            if (Board != null)
                return Board_BoardDTO;
            else
                return BadRequest(Board_BoardDTO);
        }

        [Route(BoardRoute.Update), HttpPost, Authorize]
        public async Task<ActionResult<Board_BoardDTO>> Update([FromBody] Board_BoardDTO Board_BoardDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (Board_BoardDTO.ClassroomId != null && Board_BoardDTO.ClassroomId != 0)
            {
                if (!await PermissionService.HasPermission(BoardRoute.Update, Board_BoardDTO.ClassroomId.Value))
                {
                    return Forbid();
                }
            }
            else
            {
                Board_BoardDTO.ClassroomId = null;
            }

            Board Board = ConvertDTOToEntity(Board_BoardDTO);
            Board = await BoardService.Update(Board);
            Board_BoardDTO = new Board_BoardDTO(Board);
            if (Board != null)
                return Board_BoardDTO;
            else
                return BadRequest(Board_BoardDTO);
        }

        [Route(BoardRoute.Delete), HttpPost, Authorize]
        public async Task<ActionResult<Board_BoardDTO>> Delete([FromBody] Board_BoardDTO Board_BoardDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (Board_BoardDTO.ClassroomId != null && Board_BoardDTO.ClassroomId != 0)
            {
                if (!await PermissionService.HasPermission(BoardRoute.Delete, Board_BoardDTO.ClassroomId.Value))
                {
                    return Forbid();
                }
            }
            else
            {
                Board_BoardDTO.ClassroomId = null;
            }              

            Board Board = ConvertDTOToEntity(Board_BoardDTO);
            Board = await BoardService.Delete(Board);
            Board_BoardDTO = new Board_BoardDTO(Board);
            if (Board != null)
                return Board_BoardDTO;
            else
                return BadRequest(Board_BoardDTO);
        }

        private Board ConvertDTOToEntity(Board_BoardDTO Board_BoardDTO)
        {
            Board Board = new Board();
            Board.Id = Board_BoardDTO.Id;
            Board.Code = Board_BoardDTO.Code;
            Board.Name = Board_BoardDTO.Name;
            Board.Description = Board_BoardDTO.Description;
            Board.IsFavourite = Board_BoardDTO.IsFavourite;
            Board.ImageUrl = Board_BoardDTO.ImageUrl;
            Board.CreatedAt = Board_BoardDTO.CreatedAt;
            Board.UpdatedAt = Board_BoardDTO.UpdatedAt;
            Board.ClassroomId = Board_BoardDTO.ClassroomId;
            Board.Classroom = Board_BoardDTO.Classroom == null ? null : new Classroom
            {
                Id = Board_BoardDTO.Classroom.Id,
                Code = Board_BoardDTO.Classroom.Code,
                Name = Board_BoardDTO.Classroom.Name,
                Description = Board_BoardDTO.Classroom.Description,
                CreatedAt = Board_BoardDTO.Classroom.CreatedAt,
                UpdatedAt = Board_BoardDTO.Classroom.UpdatedAt,
                DeletedAt = Board_BoardDTO.Classroom.DeletedAt,
                HomeImg = Board_BoardDTO.Classroom.HomeImg,
            };
            Board.Cards = Board_BoardDTO.Cards?.Select(x => new Card
            {
                Id = x.Id,
                BoardId = x.BoardId,
                Name = x.Name,
                Order = x.Order,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Jobs = x.Jobs?.Select(y => new Job
                {
                    Id = y.Id,
                    CardId = y.CardId,
                    Name = y.Name,
                    Description = y.Description,
                    Order = y.Order,
                    StartAt = y.StartAt,
                    EndAt = y.EndAt,
                    Color = y.Color,
                    NoTodoDone = y.NoTodoDone,
                    Todos = y.Todos?.Select(z => new Todo
                    {
                        Id = z.Id,
                        IsDone = z.IsDone,
                        JobId = z.JobId,
                        Description = z.Description,
                    }).ToList(),
                }).ToList(),
            }).ToList();
            Board.AppUserBoardMappings = Board_BoardDTO.AppUserBoardMappings?.Select(x => new AppUserBoardMapping
            {
                Id = x.Id,
                BoardId = x.BoardId,
                AppUserId = x.AppUserId,
                AppUserTypeId = x.AppUserTypeId,
            }).ToList();

            return Board;
        }

        private Card ConvertCardDTOToEntity(Board_CardDTO Board_CardDTO)
        {
            Card Card = new Card();
            Card.Id = Board_CardDTO.Id;
            Card.BoardId = Board_CardDTO.BoardId;
            Card.Name = Board_CardDTO.Name;
            Card.Order = Board_CardDTO.Order;
            Card.CreatedAt = Board_CardDTO.CreatedAt;
            Card.UpdatedAt = Board_CardDTO.UpdatedAt;
            Card.Jobs = Board_CardDTO.Jobs?.Select(y => new Job
            {
                Id = y.Id,
                CardId = y.CardId,
                Name = y.Name,
                Description = y.Description,
                Order = y.Order,
                StartAt = y.StartAt,
                EndAt = y.EndAt,
                Color = y.Color,
                NoTodoDone = y.NoTodoDone,
                Todos = y.Todos?.Select(z => new Todo
                {
                    Id = z.Id,
                    IsDone = z.IsDone,
                    JobId = z.JobId,
                    Description = z.Description,
                }).ToList(),
            }).ToList();

            return Card;
        }

        private CreateBoardsFunction ConvertDTOToEntity(Board_CreateBoardsFunctionDTO Board_CreateBoardsFunctionDTO)
        {
            CreateBoardsFunction CreateBoardsFunction = new CreateBoardsFunction();
            CreateBoardsFunction.NumberOfGroups = Board_CreateBoardsFunctionDTO.NumberOfGroups;
            CreateBoardsFunction.ClassroomId = Board_CreateBoardsFunctionDTO.ClassroomId;
            CreateBoardsFunction.AppUserIds = Board_CreateBoardsFunctionDTO.AppUserIds;

            return CreateBoardsFunction;
        }

    }
}
