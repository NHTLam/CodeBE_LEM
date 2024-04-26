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

namespace CodeBE_LEM.Services.AppUserService
{
    public interface IAppUserService
    {
        Task<List<AppUser>> List();
        Task<AppUser> Get(long Id);
        Task<bool> Create(AppUser AppUser);
        Task<AppUser> Update(AppUser AppUser);
        Task<AppUser> Delete(AppUser AppUser);
        Task<AppUser> GetPasswordHash(AppUser AppUser);
        Task<string> CreateToken(AppUser AppUser);
        Task<List<AppUser>> ListByClassroom(long ClassroomId);
    }
    public class AppUserService : IAppUserService
    {
        private IUOW UOW;
        private readonly IConfiguration Configuration;
        private readonly IBoardService BoardService;

        public AppUserService(
            IUOW UOW,
            IConfiguration Configuration,
            IBoardService BoardService
        )
        {
            this.UOW = UOW;
            this.Configuration = Configuration;
            this.BoardService = BoardService;
        }
        public async Task<bool> Create(AppUser AppUser)
        {
            try
            {
                var AppUsers = await UOW.AppUserRepository.List();
                List<string> UserNames = AppUsers.Select(a => a.UserName).ToList();
                if (UserNames.Contains(AppUser.UserName))
                {
                    return false;
                }
                AppUser.Password = BCrypt.Net.BCrypt.HashPassword(AppUser.Password);
                await UOW.AppUserRepository.Create(AppUser);
                AppUser = await UOW.AppUserRepository.Get(AppUser.Id);
                await CreateDefaultUserBoard(AppUser);
                return true;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<AppUser?> GetPasswordHash(AppUser AppUser)
        {
            try
            {
                var AppUsers = await UOW.AppUserRepository.List();
                List<string> UserNames = AppUsers.Select(a => a.UserName).ToList();
                if (UserNames.Contains(AppUser.UserName))
                {
                    var passHash = AppUsers.Where(u => u.UserName.Equals(AppUser.UserName))?.FirstOrDefault()?.Password;
                    if (passHash != null)
                    {
                        AppUser CurrentAppUser = new AppUser();
                        CurrentAppUser.UserName = AppUser.UserName;
                        CurrentAppUser.Password = passHash;
                        return CurrentAppUser;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public async Task<AppUser> Delete(AppUser AppUser)
        {
            try
            {
                await UOW.AppUserRepository.Delete(AppUser);
                return AppUser;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<AppUser> Get(long Id)
        {
            AppUser AppUser = await UOW.AppUserRepository.Get(Id);
            if (AppUser == null)
                return null;
            return AppUser;
        }

        public async Task<List<AppUser>> List()
        {
            try
            {
                List<AppUser> AppUsers = await UOW.AppUserRepository.List();
                return AppUsers;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<List<AppUser>> ListByClassroom(long ClassroomId)
        {
            try
            {
                List<AppUser> AppUsers = await UOW.AppUserRepository.List();
                AppUsers = AppUsers.Where(x => x.AppUserClassroomMappings?.Select(y => y.ClassroomId)?.Contains(ClassroomId) ?? false).ToList();
                foreach(var AppUser in AppUsers)
                {
                    if (AppUser.AppUserClassroomMappings != null && AppUser.AppUserClassroomMappings.Count > 0)
                        AppUser.AppUserClassroomMappings = AppUser.AppUserClassroomMappings.Where(x => x.ClassroomId == ClassroomId).ToList();
                }

                return AppUsers;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<AppUser> Update(AppUser AppUser)
        {
            try
            {
                var oldData = await UOW.AppUserRepository.Get(AppUser.Id);

                await UOW.AppUserRepository.Update(AppUser);

                AppUser = await UOW.AppUserRepository.Get(AppUser.Id);
                return AppUser;
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        public async Task<string> CreateToken(AppUser? AppUser)
        {
            try
            {
                AppUser = (await UOW.AppUserRepository.List()).Where(x => x.UserName == AppUser.UserName)?.FirstOrDefault();
                if (AppUser != null)
                {
                    List<Claim> claims = new List<Claim> { new Claim(ClaimTypes.Name, AppUser.UserName), new Claim(ClaimTypes.NameIdentifier, AppUser.Id.ToString()) };
                    var configToken = Configuration.GetValue<string>("AppSettings:Token");
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configToken));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
                    var token = new JwtSecurityToken(
                        claims: claims,
                        expires: DateTime.Now.AddDays(1), signingCredentials: creds
                    );

                    var jwt = new JwtSecurityTokenHandler().WriteToken(token);
                    return jwt;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {

            }
            return "";
        }

        private async Task CreateDefaultUserBoard(AppUser AppUser)
        {
            try
            {
                AppUserBoardMapping AppUserBoardMapping = new AppUserBoardMapping();
                AppUserBoardMapping.AppUserId = AppUser.Id;
                AppUserBoardMapping.AppUserTypeId = AppUserTypeEnum.OWN.Id;

                Board board = new Board();
                board.Name = $"Default Table of user {AppUser.Id}";
                board.AppUserBoardMappings = new List<AppUserBoardMapping> { AppUserBoardMapping };
                await BoardService.Create(board);
            }
            catch(Exception ex)
            { 
            
            }
        }
    }
}
