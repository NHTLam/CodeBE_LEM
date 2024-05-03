using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CodeBE_LEM.Services.AttachmentService;
using CodeBE_LEM.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using CodeBE_LEM.Controllers.AppUserController;
using System.Net.NetworkInformation;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Net;
using System.IO;
using CodeBE_LEM.Services.AppUserService;
using CloudinaryDotNet.Core;
using CodeBE_LEM.Controllers.PermissionController;
using CodeBE_LEM.Services.PermissionService;

namespace CodeBE_LEM.Controllers.AttachmentController
{
    [ApiController]
    public class AttachmentController : ControllerBase
    {
        private IAttachmentService AttachmentService;
        private IPermissionService PermissionService;

        public AttachmentController(
            IAttachmentService AttachmentService,
            IPermissionService PermissionService
        )
        {
            this.AttachmentService = AttachmentService;
            this.PermissionService = PermissionService;
        }

        [Route(AttachmentRoute.UploadFile), HttpPost, Authorize]
        public async Task<ActionResult<List<Attachment_AttachmentDTO>>> MultiUploadFile([FromForm] string classroomId, List<IFormFile> files)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            bool validClassroomId = long.TryParse(classroomId, out var classroomIdValid);
            if (!validClassroomId) return BadRequest();

            if (!await PermissionService.HasPermission(AttachmentRoute.UploadFile, classroomIdValid))
            {
                return Forbid();
            }

            List<Attachment> Attachments = await AttachmentService.MultiUploadFile(files);
            List<Attachment_AttachmentDTO> Attachment_FileDTOs = Attachments.Select(x => new Attachment_AttachmentDTO(x)).ToList();
            return Ok(Attachment_FileDTOs);
        }

        [Route(AttachmentRoute.DowloadFile), HttpPost, Authorize]
        public async Task<IActionResult> DownloadFile([FromBody] Attachment_AttachmentDTO Attachment_AttachmentDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (!await PermissionService.HasPermission(AttachmentRoute.DowloadFile, Attachment_AttachmentDTO.ClassroomId))
            {
                return Forbid();
            }

            Attachment Attachment = await AttachmentService.GetAttachment(Attachment_AttachmentDTO.Id);

            using (var client = new WebClient())
            {
                var data = await client.DownloadDataTaskAsync(Attachment.Link);
                var fileContent = new MemoryStream(data);
                var fileInfo = Attachment.Name.Split(".")[1];

                // Trả về FileResult
                return File(fileContent, AttachmentService.GetMyType(fileInfo), Attachment.Name);
            }
        }

        [Route(AttachmentRoute.DeleteFile), HttpPost, Authorize]
        public async Task<ActionResult<Attachment_AttachmentDTO>> Delete([FromBody] Attachment_AttachmentDTO Attachment_AttachmentDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Attachment Attachment = ConvertDTOToEntity(Attachment_AttachmentDTO);
            Attachment = await AttachmentService.Delete(Attachment);
            Attachment_AttachmentDTO = new Attachment_AttachmentDTO(Attachment);
            if (Attachment != null)
                return Attachment_AttachmentDTO;
            else
                return BadRequest(Attachment);
        }

        private Attachment ConvertDTOToEntity(Attachment_AttachmentDTO Attachment_AttachmentDTO)
        {
            Attachment Attachment = new Attachment();
            Attachment.Id = Attachment_AttachmentDTO.Id;
            Attachment.Name = Attachment_AttachmentDTO.Name;
            Attachment.Description = Attachment_AttachmentDTO.Description;
            Attachment.Path = Attachment_AttachmentDTO.Path;
            Attachment.Capacity = Attachment_AttachmentDTO.Capacity;
            Attachment.QuestionId = Attachment_AttachmentDTO.QuestionId;
            Attachment.OwnerId = Attachment_AttachmentDTO.OwnerId;
            Attachment.PublicId = Attachment_AttachmentDTO.PublicId;
            Attachment.Link = Attachment_AttachmentDTO.Link;

            return Attachment;
        }
    }
}
