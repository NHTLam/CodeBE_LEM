using CodeBE_LEM.Entities;
using CodeBE_LEM.Repositories;

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

    }
    public class ClassroomValidator : IClassroomValidator
    {
        private IUOW UOW;

        public ClassroomValidator(IUOW UOW)
        {
            this.UOW = UOW;
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
