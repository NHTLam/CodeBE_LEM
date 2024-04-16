using CodeBE_LEM.Entities;

namespace CodeBE_LEM.Controllers.AppUserController
{
    public class AppUser_AppUserRoleMappingDTO
    {
        public long RoleId { get; set; }

        public long AppUserId { get; set; }

        public long Id { get; set; }

        public AppUser_RoleDTO? Role { get; set; }

        public AppUser_AppUserRoleMappingDTO() { }

        public AppUser_AppUserRoleMappingDTO(AppUserRoleMapping AppUserRoleMapping)
        {
            this.Id = AppUserRoleMapping.Id;
            this.RoleId = AppUserRoleMapping.RoleId;
            this.AppUserId = AppUserRoleMapping.AppUserId;
            this.Role = AppUserRoleMapping.Role == null ? null : new AppUser_RoleDTO(AppUserRoleMapping.Role);
        }
    }
}
