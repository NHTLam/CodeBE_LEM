using CodeBE_LEM.Entities;

namespace CodeBE_LEM.Controllers.ClassroomController
{
    public class Classroom_AppUserClassroomMappingDTO
    {
        public long Id { get; set; }

        public long ClassroomId { get; set; }

        public long AppUserId { get; set; }

        public Classroom_AppUserClassroomMappingDTO() { }
        public Classroom_AppUserClassroomMappingDTO(AppUserClassroomMapping AppUserClassroomMapping)
        {
            Id = AppUserClassroomMapping.Id;
            ClassroomId = AppUserClassroomMapping.ClassroomId;
            AppUserId = AppUserClassroomMapping.AppUserId;
        }
    }
}
