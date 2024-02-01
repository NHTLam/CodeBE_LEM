using HikeBE.Entities;
using System;
using System.Collections.Generic;

namespace HikeBE.Controllers.HikeController;

public class Hike_ObservationDTO
{
    public long Id { get; set; }

    public string Time { get; set; }

    public string? Comment { get; set; }
    public long? HikeId { get; set; }
    public long? MobileObservationId { get; set; }

    public Hike_ObservationDTO() { }

    public Hike_ObservationDTO(Observation Observation)
    {
        Id = Observation.Id;
        Time = Observation.Time.ToString();
        Comment = Observation.Comment;
        HikeId = Observation.HikeId;
        MobileObservationId = Observation.MobileObservationId;
    }
}
