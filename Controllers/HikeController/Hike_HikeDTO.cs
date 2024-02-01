using HikeBE.Entities;
using System;
using System.Collections.Generic;

namespace HikeBE.Controllers.HikeController;

public partial class Hike_HikeDTO
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string Date { get; set; }

    public bool CanParking { get; set; }

    public decimal Length { get; set; }

    public int HikeLevel { get; set; }

    public string? Description { get; set; }

    public decimal? Turn { get; set; }

    public int? RepeatInDay { get; set; }

    public long? TypeId { get; set; }
    public long? MobileHikeId { get; set; }

    public List<Hike_ImgDTO>? Imgs { get; set; }
    public List<Hike_ObservationDTO>? Observations { get; set; }

    //public Hike_HikeTypeDTO? Type { get; set; }

    public Hike_HikeDTO(){}

    public Hike_HikeDTO(Hike Hike)
    {
        this.Id = Hike.Id;
        this.Name = Hike.Name;
        this.Location = Hike.Location;
        this.Date = Hike.Date.ToString();
        this.CanParking = Hike.CanParking;
        this.Length = Hike.Length;
        this.HikeLevel = Hike.HikeLevel;
        this.Description = Hike.Description;
        this.Turn = Hike.Turn;
        this.RepeatInDay = Hike.RepeatInDay;
        this.TypeId = Hike.TypeId;
        this.MobileHikeId = Hike.MobileHikeId;
        //this.Type = Hike.Type == null ? null : new Hike_HikeTypeDTO(Hike.Type);
        this.Imgs = (Hike.Imgs == null || Hike.Imgs.Count() == 0) ? null : Hike.Imgs?.Select(t => new Hike_ImgDTO(t)).ToList();
        this.Observations = (Hike.Observations == null || Hike.Observations.Count() == 0) ? null : Hike.Observations?.Select(t => new Hike_ObservationDTO(t)).ToList();
    }
}
