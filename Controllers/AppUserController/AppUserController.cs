using CodeBE_LEM.Controllers.PermissionController;
using CodeBE_LEM.Entities;
using CodeBE_LEM.Models;
using CodeBE_LEM.Services.AppUserService;
using CodeBE_LEM.Services.PermissionService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Linq;

namespace CodeBE_LEM.Controllers.AppUserController
{
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private IAppUserService AppUserService;
        private IPermissionService PermissionService;

        public AppUserController(
            IAppUserService AppUserService,
            IPermissionService PermissionService
        )
        {
            this.AppUserService = AppUserService;
            this.PermissionService = PermissionService;
        }

        [Route(AppUserRoute.List), HttpPost, Authorize]
        public async Task<ActionResult<List<AppUser_AppUserDTO>>> List()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<AppUser> AppUsers = await AppUserService.List();
            List<AppUser_AppUserDTO> AppUser_AppUserDTOs = AppUsers.Select(x => new AppUser_AppUserDTO(x)).ToList();
            return AppUser_AppUserDTOs;
        }

        [Route(AppUserRoute.GetUserId), HttpPost, Authorize]
        public async Task<ActionResult<long>> GetUserId()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            long id = PermissionService.GetAppUserId();
            return id;
        }

        [Route(AppUserRoute.Get), HttpPost, Authorize]
        public async Task<ActionResult<AppUser_AppUserDTO>> Get([FromBody] AppUser_AppUserDTO AppUser_AppUserDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            AppUser AppUser = ConvertDTOToEntity(AppUser_AppUserDTO);
            AppUser = await AppUserService.Get(AppUser.Id);
            AppUser_AppUserDTO = new AppUser_AppUserDTO(AppUser);
            return AppUser_AppUserDTO;
        }

        [Route(AppUserRoute.Register), HttpPost]
        public async Task<ActionResult<bool>> Register([FromBody] AppUser_AppUserDTO AppUser_AppUserDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            AppUser AppUser = ConvertDTOToEntity(AppUser_AppUserDTO);
            bool isRegisterSuccess = await AppUserService.Create(AppUser);
            if (isRegisterSuccess)
                return true;
            else
                return BadRequest("AppUsername already exists");
        }

        [Route(AppUserRoute.Login), HttpPost]
        public async Task<ActionResult<bool>> Login([FromBody] AppUser_AppUserDTO AppUser_AppUserDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            AppUser AppUser = ConvertDTOToEntity(AppUser_AppUserDTO);
            AppUser CurrentAppUser = await AppUserService.GetPasswordHash(AppUser);

            if (CurrentAppUser == null)
                return BadRequest("AppUser not found");

            if (!BCrypt.Net.BCrypt.Verify(AppUser.Password, CurrentAppUser.Password))
                return BadRequest("Wrong password");

            var token = await AppUserService.CreateToken(AppUser);

            return Ok(token);
        }

        [Route(AppUserRoute.Update), HttpPost, Authorize]
        public async Task<ActionResult<AppUser_AppUserDTO>> Update([FromBody] AppUser_AppUserDTO AppUser_AppUserDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            AppUser AppUser = ConvertDTOToEntity(AppUser_AppUserDTO);
            AppUser = await AppUserService.Update(AppUser);
            AppUser_AppUserDTO = new AppUser_AppUserDTO(AppUser);
            if (AppUser != null)
                return AppUser_AppUserDTO;
            else
                return BadRequest(AppUser);
        }

        [Route(AppUserRoute.Delete), HttpPost, Authorize]
        public async Task<ActionResult<AppUser_AppUserDTO>> Delete([FromBody] AppUser_AppUserDTO AppUser_AppUserDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            AppUser AppUser = ConvertDTOToEntity(AppUser_AppUserDTO);
            AppUser = await AppUserService.Delete(AppUser);
            AppUser_AppUserDTO = new AppUser_AppUserDTO(AppUser);
            if (AppUser != null)
                return AppUser_AppUserDTO;
            else
                return BadRequest(AppUser);
        }

        private AppUser ConvertDTOToEntity(AppUser_AppUserDTO AppUser_AppUserDTO)
        {
            AppUser AppUser = new AppUser();
            AppUser.Id = AppUser_AppUserDTO.Id;
            AppUser.FullName = AppUser_AppUserDTO.FullName;
            AppUser.UserName = AppUser_AppUserDTO.UserName;
            AppUser.Email = AppUser_AppUserDTO.Email;
            AppUser.Gender = AppUser_AppUserDTO.Gender;
            AppUser.Phone = AppUser_AppUserDTO.Phone;
            AppUser.Password = AppUser_AppUserDTO.Password;
            AppUser.StatusId = AppUser_AppUserDTO.StatusId ?? 0;
            AppUser.AppUserRoleMappings = AppUser_AppUserDTO.AppUserRoleMappings?.Select(x => new AppUserRoleMapping
            {
                Id = x.Id,
                RoleId = x.RoleId,
                AppUserId = x.AppUserId
            }).ToList();

            return AppUser;
        }
    }
}
