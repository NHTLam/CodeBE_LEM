using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System.Net.NetworkInformation;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Azure;
using System.IO;
using Microsoft.VisualBasic;
using HikeBE.Services.SHike;
using HikeBE.Services.SImg;
using HikeBE.Entities;
using HikeBE.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authorization;

namespace HikeBE.Controllers.HikeController
{
    [ApiController]
    public class HikeController : ControllerBase
    {
        private IHikeService HikeService;
        private IImgService ImgService;

        public HikeController(
            IHikeService HikeService,
            IImgService ImgService
        )
        {
            this.HikeService = HikeService;
            this.ImgService = ImgService;
        }

        [Route(HikeRoute.List), HttpPost, Authorize]
        public async Task<ActionResult<List<Hike_HikeDTO>>> List()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<Hike> Hikes = await HikeService.List();
            List<Hike_HikeDTO> Hike_HikeDTOs = Hikes
                .Select(c => new Hike_HikeDTO(c)).ToList();

            return Hike_HikeDTOs;
        }

        [Route(HikeRoute.Get), HttpPost, Authorize]
        public async Task<ActionResult<Hike_HikeDTO>> Get([FromBody] Hike_HikeDTO Hike_HikeDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Hike Hike = await HikeService.Get(Hike_HikeDTO.Id);
            Hike_HikeDTO = new Hike_HikeDTO(Hike);
            return Hike_HikeDTO;
        }

        [Route(HikeRoute.Create), HttpPost, Authorize]
        public async Task<ActionResult<Hike_HikeDTO>> Create([FromBody] Hike_HikeDTO Hike_HikeDTO)
        {
           if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Hike Hike = ConvertDTOToEntity(Hike_HikeDTO);

            Hike = await HikeService.Create(Hike);
            Hike_HikeDTO = new Hike_HikeDTO(Hike);
            if (Hike != null)
                return Hike_HikeDTO;
            else
                return BadRequest(Hike_HikeDTO);
        }

        [Route(HikeRoute.Update), HttpPost, Authorize]
        public async Task<ActionResult<Hike_HikeDTO>> Update([FromBody] Hike_HikeDTO Hike_HikeDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Hike Hike = ConvertDTOToEntity(Hike_HikeDTO);
            Hike = await HikeService.Update(Hike);
            Hike_HikeDTO = new Hike_HikeDTO(Hike);
            if (Hike != null)
                return Hike_HikeDTO;
            else
                return BadRequest(Hike_HikeDTO);
        }

        [Route(HikeRoute.Delete), HttpPost, Authorize]
        public async Task<ActionResult<Hike_HikeDTO>> Delete([FromBody] Hike_HikeDTO Hike_HikeDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Hike Hike = ConvertDTOToEntity(Hike_HikeDTO);
            Hike = await HikeService.Delete(Hike);
            Hike_HikeDTO = new Hike_HikeDTO(Hike);
            if (Hike != null)
                return Hike_HikeDTO;
            else
                return BadRequest(Hike_HikeDTO);
        }

        [Route(HikeRoute.UploadImg), HttpPost, Authorize]
        public async Task<ActionResult<List<Hike_ImgDTO>>> UploadImage(List<IFormFile> formImgs)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            List<Hike_ImgDTO> Hike_ImgDTOs = new List<Hike_ImgDTO>();
            foreach (var Img in formImgs)
            {
                MemoryStream memoryStream = new MemoryStream();
                //await Img.CopyToAsync(memoryStream);
                Img.CopyTo(memoryStream);
                Img HikeImg = new Img();
                HikeImg.Name = Img.FileName;
                HikeImg.HikeImg = memoryStream.ToArray();
                HikeImg = await ImgService.Create(HikeImg);
                if (HikeImg == null)
                    return BadRequest();
                Hike_ImgDTO Hike_ImgDTO = new Hike_ImgDTO(HikeImg);
                Hike_ImgDTOs.Add(Hike_ImgDTO);
            }
            return Ok(Hike_ImgDTOs);
        }

        [Route(HikeRoute.GetDataFromMobile), HttpPost]
        public async Task<ActionResult<bool>> GetDataFromMobile([FromBody] List<Hike_HikeDTO> Hike_HikeDTOs)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<Hike> Hikes = Hike_HikeDTOs.Select(x => ConvertDTOToEntity(x)).ToList();

            bool isGetDataSuccess = await HikeService.GetDataFromMobile(Hikes);
            if (isGetDataSuccess != false)
                return Ok(true);
            else
                return BadRequest(isGetDataSuccess);
        }

        [Route(HikeRoute.BulkDeleteImg), HttpPost, Authorize]
        public async Task<ActionResult<bool>> BulkDeleteImg([FromBody] Hike_HikeDTO Hike_HikeDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Hike Hike = ConvertDTOToEntity(Hike_HikeDTO);

            bool isGetDataSuccess = await ImgService.BulkDelete(Hike);
            if (isGetDataSuccess != false)
                return Ok(true);
            else
                return BadRequest(isGetDataSuccess);
        }

        private Hike ConvertDTOToEntity(Hike_HikeDTO Hike_HikeDTO)
        {
            Hike Hike = new Hike();
            Hike.Id = Hike_HikeDTO.Id;
            Hike.Name = Hike_HikeDTO.Name;
            Hike.Location = Hike_HikeDTO.Location;
            Hike.Date = ConvertDateString(Hike_HikeDTO.Date);
            Hike.CanParking = Hike_HikeDTO.CanParking;
            Hike.Length = Hike_HikeDTO.Length;
            Hike.HikeLevel = Hike_HikeDTO.HikeLevel;
            Hike.Description = Hike_HikeDTO.Description;
            Hike.Turn = Hike_HikeDTO.Turn;
            Hike.RepeatInDay = Hike_HikeDTO.RepeatInDay;
            Hike.TypeId = Hike_HikeDTO.TypeId;
            Hike.MobileHikeId = Hike_HikeDTO.MobileHikeId;
            Hike.Imgs = Hike_HikeDTO.Imgs?.Select(x => new Img
            {
                Id = x.Id,
                Name = x.Name,
                HikeImg = x.StringHikeImg != null ? Convert.FromBase64String(x.StringHikeImg) : null,
                HikeId = x.HikeId,
                MobileImgId = x.MobileImgId
            }).ToList();
            Hike.Observations = Hike_HikeDTO.Observations?.Select(x => new Observation
            {
                Id = x.Id,
                Time = ConvertDateString(x.Time),
                Comment = x.Comment,
                HikeId = x.HikeId,
                MobileObservationId = x.MobileObservationId
            }).ToList();

            return Hike;
        }

        private DateTime ConvertDateString(string dateString)
        {
            try
            {
                DateTime date = DateTime.ParseExact(dateString, "dd-MM-yyyy", null);
                return date;
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }
    }
}
