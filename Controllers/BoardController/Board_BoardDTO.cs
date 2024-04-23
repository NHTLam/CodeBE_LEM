using CodeBE_LEM.Entities;
using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Controllers.BoardController;

public class Board_BoardDTO
{
    public long Id { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsFavourite { get; set; }

    public long? ClassroomId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public Board_ClassroomDTO? Classroom { get; set; }

    public List<Board_CardDTO>? Cards { get; set; } = new List<Board_CardDTO>();
    public List<Board_AppUserBoardMappingDTO>? AppUserBoardMappings { get; set; } = new List<Board_AppUserBoardMappingDTO>();

    public Board_BoardDTO() { }

    public Board_BoardDTO(Board Board)
    {
        Id = Board.Id;
        Code = Board.Code;
        Name = Board.Name;
        Description = Board.Description;
        IsFavourite = Board.IsFavourite;
        ImageUrl = Board.ImageUrl;
        CreatedAt = Board.CreatedAt;
        UpdatedAt = Board.UpdatedAt;
        DeletedAt = Board.DeletedAt;
        ClassroomId = Board.ClassroomId ?? 0;
        Classroom = Board.Classroom == null ? null : new Board_ClassroomDTO(Board.Classroom);
        Cards = Board.Cards?.Select(x => new Board_CardDTO(x)).ToList();
        AppUserBoardMappings = Board.AppUserBoardMappings?.Select(x => new Board_AppUserBoardMappingDTO(x)).ToList();
    }
}
