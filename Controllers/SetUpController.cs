using CodeBE_LEM.Entities;
using CodeBE_LEM.Enums;
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
        public async Task<ActionResult> Init()
        {
            await InitPermission();
            return Ok();
        }

        private async Task InitPermission()
        {
            await PermissionService.Init();
            
            await UOW.PermissionRepository.DeleteRoleAuto();
            Role RoleTeacher = new Role();
            RoleTeacher.Name = "Teacher";
            RoleTeacher.Description = "Vai trò giáo viên sẽ có toàn quyền trong lớp học";
            RoleTeacher.RoleTypeId = RoleTypeEnum.AUTO.Id;
            await PermissionService.CreateRole(RoleTeacher);

            Role RoleStudent = new Role();
            RoleStudent.Name = "Student";
            RoleStudent.Description = "Vai trò học sinh trong lớp học";
            RoleStudent.RoleTypeId = RoleTypeEnum.AUTO.Id;
            await PermissionService.CreateRole(RoleStudent);
        }
    }
}
