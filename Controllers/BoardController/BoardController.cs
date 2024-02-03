using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CodeBE_LEM.Services.BoardService;
using CodeBE_LEM.Entities;

namespace CodeBE_LEM.Controllers.BoardController
{
    [ApiController]
    public class BoardController : ControllerBase
    {
        private IBoardService BoardService;

        public BoardController(
            IBoardService BoardService
        )
        {
            this.BoardService = BoardService;
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

        [Route(BoardRoute.Get), HttpPost, Authorize]
        public async Task<ActionResult<Board_BoardDTO>> Get([FromBody] Board_BoardDTO Board_BoardDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Board Board = await BoardService.Get(Board_BoardDTO.Id);
            Board_BoardDTO = new Board_BoardDTO(Board);
            return Board_BoardDTO;
        }

        [Route(BoardRoute.Create), HttpPost, Authorize]
        public async Task<ActionResult<Board_BoardDTO>> Create([FromBody] Board_BoardDTO Board_BoardDTO)
        {
           if (!ModelState.IsValid)
                return BadRequest(ModelState);

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
            Board.CreatedAt = Board_BoardDTO.CreatedAt;
            Board.UpdatedAt = Board_BoardDTO.UpdatedAt;
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
                    PlanTime = y.PlanTime,
                    Color = y.Color,
                    NoTodoDone = y.NoTodoDone,
                    Todos = y.Todos?.Select(z => new Todo
                    {
                        Id = z.Id,
                        CompletePercent = z.CompletePercent,
                        JobId = z.JobId,
                        Description = z.Description,
                    }).ToList(),
                }).ToList(),
            }).ToList();

            return Board;
        }
    }
}
