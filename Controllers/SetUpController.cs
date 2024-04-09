using CodeBE_LEM.Entities;
using CodeBE_LEM.Models;
using CodeBE_LEM.Repositories;
using CodeBE_LEM.Services.PermissionService;
using Microsoft.AspNetCore.Mvc;

namespace CodeBE_LEM.Controllers
{
    public class SetUpController : ControllerBase
    {
        private DataContext DataContext;
        private IPermissionService PermissionService;
        private IUOW UOW;
        public SetUpController(DataContext DataContext, IPermissionService PermissionService, IUOW uOW)
        {
            this.DataContext = DataContext;
            this.PermissionService = PermissionService;
            UOW = uOW;
        }

        [HttpGet, Route("lem/setup/init")]
        public async Task<ActionResult> InitEnum()
        {
            await InitPermission();
            await InitPermissionForAdmin();
            return Ok();
        }

        private async Task InitPermission()
        {
            await PermissionService.Init();
        }

        private async Task InitPermissionForAdmin()
        {
            await DataContext.PermissionRoleMappings.Where(x => x.RoleId == 1).DeleteFromQueryAsync();
            List<Permission> AllPermissions = await PermissionService.ListPermission();
            List<PermissionRoleMappingDAO> PermissonRoleMappingDAOs = new List<PermissionRoleMappingDAO>();
            foreach (Permission permission in AllPermissions)
            {
                PermissionRoleMappingDAO PermissionRoleMappingDAO = new PermissionRoleMappingDAO();
                PermissionRoleMappingDAO.RoleId = 1; //Fix cứng id của admin là 1
                PermissionRoleMappingDAO.PermissionId = permission.Id;
                PermissonRoleMappingDAOs.Add(PermissionRoleMappingDAO);
            }
            await DataContext.AddRangeAsync(PermissonRoleMappingDAOs);
            await DataContext.SaveChangesAsync();

            var User = await UOW.AppUserRepository.Get(2);
            if (User.AppUserRoleMappings == null || User.AppUserRoleMappings.Count == 0)
            {
                AppUserRoleMappingDAO AppUserRoleMappingDAO = new AppUserRoleMappingDAO();
                AppUserRoleMappingDAO.RoleId = 1;
                AppUserRoleMappingDAO.AppUserId = 2;
                DataContext.AppUserRoleMappings.Add(AppUserRoleMappingDAO);
                await DataContext.SaveChangesAsync();
            }
        }
    }
}
