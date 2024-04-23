using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using CodeBE_LEM.Models;
using CodeBE_LEM.Repositories;
using CodeBE_LEM.Entities;
using CodeBE_LEM.Services.BoardService;
using CodeBE_LEM.Enums;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using RestSharp;
using CodeBE_LEM.Services.PermissionService;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CodeBE_LEM.Services.AttachmentService
{
    public interface IAttachmentService
    {
        Task<List<Attachment>> MultiUploadFile(List<IFormFile> files, long QuestionId);
        Account GetAccount();
        string GetMyType(string fileFormat);
        Task<Attachment> GetAttachment(long AttachmentId);
        Task<Attachment> Delete(Attachment Attachment);
    }
    public class AttachmentService : IAttachmentService
    {
        private IUOW UOW;
        private IPermissionService PermissionService;
        private const string CLOUD_NAME = "dajnju9vi";
        private const string API_KEY = "184944748294369";
        private const string API_SECRET = "80g1sTjm9kKp4fqwUN2KIldrc9o";

        public AttachmentService(
            IUOW UOW,
            IPermissionService PermissionService
        )
        {
            this.UOW = UOW;
            this.PermissionService = PermissionService;
        }

        public async Task<List<Attachment>> MultiUploadFile(List<IFormFile> files, long QuestionId)
        {
            try
            {
                List<Attachment> Attachments = new List<Attachment>();
                foreach (var file in files)
                {
                    Attachment Attachment = new Attachment();
                    Attachment.QuestionId = QuestionId;
                    Attachment.Capacity = file.Length.ToString();
                    Attachment.Name = file.FileName;
                    Attachment.OwnerId = PermissionService.GetAppUserId();
                    Attachment = await CloudinaryStorage(Attachment, file);
                    Attachments.Add(Attachment);
                    if (Attachment == null)
                        return new List<Attachment>();
                }

                await UOW.AttachmentRepository.BulkMerge(Attachments);
                return Attachments;
            }
            catch (Exception ex)
            {
            
            }
            return new List<Attachment>();
        }

        public async Task<Attachment> GetAttachment(long AttachmentId)
        {
            try
            {
                Attachment Attachment = await UOW.AttachmentRepository.Get(AttachmentId);
                if (Attachment == null)
                    return null;
                return Attachment;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public Account GetAccount()
        {
            return new Account(CLOUD_NAME, API_KEY, API_SECRET);
        }

        public async Task<Attachment> Delete(Attachment Attachment)
        {
            try
            {
                await UOW.AttachmentRepository.Delete(Attachment);
                return Attachment;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        private async Task<Attachment> CloudinaryStorage(Attachment Attachment, IFormFile file)
        {
            Account account = new Account(CLOUD_NAME, API_KEY, API_SECRET);
            Cloudinary cloudinary = new Cloudinary(account);

            // Đọc dữ liệu từ IFormFile trực tiếp
            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new RawUploadParams()
                {
                    File = new FileDescription(file.FileName, stream)
                };

                var res = await cloudinary.UploadAsync(uploadParams);
                if (res != null)
                {
                    Attachment.PublicId = res.PublicId.ToString();
                    Attachment.Link = res.Uri.ToString();
                    return Attachment;
                }
                else
                {
                    // Xử lý lỗi ở đây, tùy thuộc vào yêu cầu của ứng dụng
                    return null;
                }
            }
        }

        public string GetMyType(string fileFormat)
        {
            if (string.IsNullOrEmpty(fileFormat))
            {
                return "application/octet-stream";
            }
            // Bạn có thể mở rộng danh sách các loại tệp và MIME type tương ứng ở đây
            switch (fileFormat.ToLower())
            {
                case "pdf":
                    return "application/pdf";
                case "png":
                    return "image/png";
                case "jpeg":
                case "jpg":
                    return "image/jpeg";
                case "docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                // Thêm các loại tệp và MIME type tương ứng ở đây
                default:
                    return "application/octet-stream"; // Loại mặc định
            }
        }
    }
}
