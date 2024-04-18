using CodeBE_LEM.Entities;
using CodeBE_LEM.Repositories;
using CodeBE_LEM.Services.PermissionService;

namespace CodeBE_LEM.Services.ClassroomService
{
    public interface IClassroomValidator
    {
        Task Get(Classroom Classroom);
        Task<bool> Create(Classroom Classroom);
        Task<bool> Update(Classroom Classroom);
        Task<bool> Delete(Classroom Classroom);
        Task GetClassEvent(ClassEvent ClassEvent);
        Task<bool> CreateClassEvent(ClassEvent ClassEvent);
        Task<bool> UpdateClassEvent(ClassEvent ClassEvent);
        Task<bool> DeleteClassEvent(ClassEvent ClassEvent);
        Task<bool> Join(string code);
    }
    public class ClassroomValidator : IClassroomValidator
    {
        private IUOW UOW;
        private IPermissionService PermissionService;

        public ClassroomValidator(IUOW UOW, IPermissionService PermissionService)
        {
            this.UOW = UOW;
            this.PermissionService = PermissionService;
        }
        public async Task<bool> Create(Classroom Classroom)
        {
            return true;
        }

        public async Task<bool> Delete(Classroom Classroom)
        {
            return true;
        }

        public async Task Get(Classroom Classroom)
        {
            
        }

        public async Task<bool> Join(string code)
        {
            var CurrentUserClassIds = await UOW.ClassroomRepository.ListClassroomIdByUserId(PermissionService.GetAppUserId());
            List<Classroom> Classrooms = await UOW.ClassroomRepository.List();
            List<string> ClassrooCodes = Classrooms.Select(x => x.Code).ToList();
            if (ClassrooCodes.Contains(code))
            {
                var ClassWantJoin = Classrooms.FirstOrDefault(x => x.Code == code);
                if (CurrentUserClassIds.Contains(ClassWantJoin.Id)) //Nếu đã join class
                {
                    return false;
                }
            }
            return true;
        }
                

        public async Task<bool> Update(Classroom Classroom)
        {
            return true;
        }

        public async Task<bool> CreateClassEvent(ClassEvent ClassEvent)
        {
            return true;
        }

        public async Task<bool> DeleteClassEvent(ClassEvent ClassEvent)
        {
            return true;
        }

        public async Task GetClassEvent(ClassEvent ClassEvent)
        {

        }

        public async Task<bool> UpdateClassEvent(ClassEvent ClassEvent)
        {
            return true;
        }

    }
}
