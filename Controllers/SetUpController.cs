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
            RoleTeacher.Description = "Vai trò giáo viên sẽ có toàn quyền trong lớp học";
            RoleTeacher.RoleTypeId = RoleTypeEnum.AUTO.Id;
            await PermissionService.CreateRole(RoleTeacher);

            Role RoleStudent = new Role();
            RoleStudent.Name = "Student";
            RoleStudent.Description = "Vai trò học sinh trong lớp học";
            RoleStudent.RoleTypeId = RoleTypeEnum.AUTO.Id;
            await PermissionService.CreateRole(RoleStudent);
        }

        private async Task SetPermissionForSystemRole()
        {
            List<Permission> Permissions = await UOW.PermissionRepository.ListPermission();
            List<Permission> PermissionForStudent = Permissions.Where(x => ActionEnum.ActionEnumListForStudent.Select(y => y.Name).Contains(x.Name)).ToList();
            List<Role> Roles = await UOW.PermissionRepository.ListSystemRole();

            #region Assign Permissions for role Teacher
            var TeacherRole = Roles.Where(x => x.Name == "Teacher").FirstOrDefault();
            TeacherRole = InitRolePermission(Permissions, TeacherRole);
            #endregion

            #region Assign Permissions for role Student
            var StudentRole = Roles.Where(x => x.Name == "Student").FirstOrDefault();
            StudentRole = InitRolePermission(PermissionForStudent, StudentRole);
            #endregion
        }

        private Role InitRolePermission(List<Permission> Permissions, Role? Role)
        {
            List<PermissionRoleMapping> PermissionRoleMappings = new List<PermissionRoleMapping>();
            foreach (var permission in Permissions)
            {
                PermissionRoleMapping PermissionRoleMapping = new PermissionRoleMapping();
                PermissionRoleMapping.RoleId = Role.Id;
                PermissionRoleMapping.PermissionId = permission.Id;
                PermissionRoleMappings.Add(PermissionRoleMapping);
            }

            Role.PermissionRoleMappings = PermissionRoleMappings;
            return Role;
        }
    }
}
