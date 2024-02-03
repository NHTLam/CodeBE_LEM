using CodeBE_LEM.Entities;
using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Controllers.BoardController;

public class Board_CardDTO
{
    public long Id { get; set; }

    public long BoardId { get; set; }

    public string Name { get; set; } = null!;

    public int? Order { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public List<Board_JobDTO> Jobs { get; set; } = new List<Board_JobDTO>();

    public Board_CardDTO() { }

    public Board_CardDTO(Card Card)
    {
        Id = Card.Id;
        BoardId = Card.BoardId;
        Name = Card.Name;
        Order = Card.Order;
        CreatedAt = Card.CreatedAt;
        UpdatedAt = Card.UpdatedAt;
        DeletedAt = Card.DeletedAt;
        Jobs = Card.Jobs?.Select(x => new Board_JobDTO(x)).ToList();
    }
}
