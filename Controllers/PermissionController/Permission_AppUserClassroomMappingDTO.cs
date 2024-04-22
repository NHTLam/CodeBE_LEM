using CodeBE_LEM.Entities;

namespace CodeBE_LEM.Controllers.PermissionController
{
    public class Permission_AppUserClassroomMappingDTO
    {
        public long Id { get; set; }

        public long ClassroomId { get; set; }

        public long AppUserId { get; set; }

        public long? RoleId { get; set; }

        public Permission_AppUserClassroomMappingDTO() { }
        public Permission_AppUserClassroomMappingDTO(AppUserClassroomMapping AppUserClassroomMapping)
        {
            Id = AppUserClassroomMapping.Id;
            ClassroomId = AppUserClassroomMapping.ClassroomId;
            AppUserId = AppUserClassroomMapping.AppUserId;
            RoleId = AppUserClassroomMapping.RoleId;
        }
    }
}
