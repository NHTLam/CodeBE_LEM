using CodeBE_LEM.Common;
using CodeBE_LEM.Entities;
using CodeBE_LEM.Repositories;

namespace CodeBE_LEM.Services.ClassroomService
{
    public interface IClassroomService
    {
        Task<List<Classroom>> List(FilterDTO FilterDTO);
        Task<List<Classroom>> ListOwn(long AppUserId);
        Task<Classroom> Get(long Id);
        Task<Classroom> Create(Classroom Classroom);
        Task<Classroom> Update(Classroom Classroom);
        Task<Classroom> Delete(Classroom Classroom);
    }
    public class ClassroomService : BaseService<Classroom>, IClassroomService
    {
        private IUOW UOW;
        private IClassroomValidator ClassroomValidator;
        public ClassroomService(
            IUOW UOW,
            IClassroomValidator ClassroomValidator
        )
        {
            this.UOW = UOW;
            this.ClassroomValidator = ClassroomValidator;
        }
        public async Task<Classroom> Create(Classroom Classroom)
        {
            if (!await ClassroomValidator.Create(Classroom))
                return Classroom;

            try
            {
                Classroom.Code = string.Empty;
                Classroom.CreatedAt = DateTime.Now;
                Classroom.UpdatedAt = DateTime.Now;
                Classroom.DeletedAt = null;
                await UOW.ClassroomRepository.Create(Classroom);
                await BuildCode(Classroom);
                Classroom = await UOW.ClassroomRepository.Get(Classroom.Id);
                return Classroom;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<Classroom> Delete(Classroom Classroom)
        {
            if (!await ClassroomValidator.Delete(Classroom))
                return Classroom;

            try
            {
                Classroom = await Get(Classroom.Id);
                await UOW.ClassroomRepository.Delete(Classroom);
                return Classroom;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<Classroom> Get(long Id)
        {
            Classroom Classroom = await UOW.ClassroomRepository.Get(Id);
            if (Classroom == null)
                return null;
            await ClassroomValidator.Get(Classroom);
            return Classroom;
        }

        public async Task<List<Classroom>> List(FilterDTO FilterDTO)
        {
            try
            {
                List<Classroom> Classrooms = await UOW.ClassroomRepository.List();

                Classrooms = FilterData(Classrooms, FilterDTO);

                return Classrooms;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<List<Classroom>> ListOwn(long AppUserId)
        {
            try
            {
                List<long> ValidClassIds = await UOW.ClassroomRepository.ListClassroomIdByUserId(AppUserId);
                List<Classroom> Classrooms = await UOW.ClassroomRepository.List();

                FilterDTO FilterDTO = new FilterDTO();
                FilterDTO.Id = ValidClassIds;
                Classrooms = FilterData(Classrooms, FilterDTO);

                return Classrooms;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<Classroom> Update(Classroom Classroom)
        {
            if (!await ClassroomValidator.Update(Classroom))
                return Classroom;
            try
            {
                var oldData = await UOW.ClassroomRepository.Get(Classroom.Id);
                Classroom.CreatedAt = oldData.CreatedAt;
                Classroom.UpdatedAt = DateTime.Now;
                Classroom.DeletedAt = null;
                await UOW.ClassroomRepository.Update(Classroom);
                await BuildCode(Classroom);
                Classroom = await UOW.ClassroomRepository.Get(Classroom.Id);
                return Classroom;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        private async Task BuildCode(Classroom Classroom)
        {
            Classroom.Code = "C" + Classroom.Id;
            await UOW.ClassroomRepository.UpdateCode(Classroom);
        }
    }
}
