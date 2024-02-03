using CodeBE_LEM.Entities;
using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Controllers.BoardController;

public class Board_BoardDTO
{
    public long Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsFavourite { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public List<Board_CardDTO> Cards { get; set; } = new List<Board_CardDTO>();

    public Board_BoardDTO() { }

    public Board_BoardDTO(Board Board)
    {
        Id = Board.Id;
        Code = Board.Code;
        Name = Board.Name;
        Description = Board.Description;
        IsFavourite = Board.IsFavourite;
        CreatedAt = Board.CreatedAt;
        UpdatedAt = Board.UpdatedAt;
        DeletedAt = Board.DeletedAt;
        Cards = Board.Cards?.Select(x => new Board_CardDTO(x)).ToList();
    }
}
