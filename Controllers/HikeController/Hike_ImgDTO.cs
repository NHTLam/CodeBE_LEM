using HikeBE.Entities;
using System;
using System.Collections.Generic;

namespace HikeBE.Controllers.HikeController;

public class Hike_ImgDTO
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? StringHikeImg { get; set; }
    public long? HikeId { get; set; }
    public long? MobileImgId { get; set; }

    public Hike_ImgDTO(){}

    public Hike_ImgDTO(Img img)
    {
        Id = img.Id;
        Name = img.Name;
        StringHikeImg = img.HikeImg != null ? Convert.ToBase64String(img.HikeImg) : null;
        HikeId = img.HikeId;
        MobileImgId = img.MobileImgId;
    }


}
