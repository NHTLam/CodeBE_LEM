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
            await SetPermissionForSystemRole();
            return Ok();
        }

        private async Task InitPermission()
        {
            await PermissionService.Init();

            await UOW.PermissionRepository.DeleteRoleAuto();
            Role RoleTeacher = new Role();
            RoleTeacher.Name = "Teacher";
            RoleTeacher.Description = "The teacher role has full authority in the classroom";
            RoleTeacher.RoleTypeId = RoleTypeEnum.AUTO.Id;
            await PermissionService.CreateRoleSystem(RoleTeacher, null);

            Role RoleStudent = new Role();
            RoleStudent.Name = "Student";
            RoleStudent.Description = "Student role in the classroom";
            RoleStudent.RoleTypeId = RoleTypeEnum.AUTO.Id;
            await PermissionService.CreateRoleSystem(RoleStudent, null);
        }

        private async Task SetPermissionForSystemRole()
        {
            List<Permission> Permissions = await UOW.PermissionRepository.ListPermission();
            List<Permission> PermissionForStudent = Permissions.Where(x => ActionEnum.ActionEnumListForStudent.Select(y => y.Name).Contains(x.Name)).ToList();
            List<Role> Roles = await UOW.PermissionRepository.ListSystemRole();

            #region Assign Permissions for role Teacher
            var TeacherRole = Roles.Where(x => x.Name == "Teacher").FirstOrDefault();
            List<PermissionRoleMapping> NewPermissionRoleMappings = InitRolePermission(Permissions, TeacherRole);
            #endregion

            #region Assign Permissions for role Student
            var StudentRole = Roles.Where(x => x.Name == "Student").FirstOrDefault();
            NewPermissionRoleMappings.AddRange(InitRolePermission(PermissionForStudent, StudentRole));
            #endregion
            await UOW.PermissionRepository.BulkMergePermissionRoleMappings(NewPermissionRoleMappings);
        }

        private List<PermissionRoleMapping> InitRolePermission(List<Permission> Permissions, Role? Role)
        {
            List<PermissionRoleMapping> PermissionRoleMappings = new List<PermissionRoleMapping>();
            foreach (var permission in Permissions)
            {
                PermissionRoleMapping PermissionRoleMapping = new PermissionRoleMapping();
                PermissionRoleMapping.RoleId = Role.Id;
                PermissionRoleMapping.PermissionId = permission.Id;
                PermissionRoleMappings.Add(PermissionRoleMapping);
            }
            return PermissionRoleMappings;
        }
    }
}
