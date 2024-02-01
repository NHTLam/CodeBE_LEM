using HikeBE.Entities;
using System;
using System.Collections.Generic;

namespace HikeBE.Controllers.HikeController;

public partial class Hike_HikeTypeDTO
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public Hike_HikeTypeDTO(){}

    public Hike_HikeTypeDTO(HikeType HikeType)
    {
        Id = HikeType.Id;
        Name = HikeType.Name;
        Code = HikeType.Code;
    }
}
