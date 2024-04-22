using CodeBE_LEM.Common;
using CodeBE_LEM.Entities;
using CodeBE_LEM.Repositories;
using CodeBE_LEM.Services.PermissionService;

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
        Task<bool> Join(string code);
    }
    public class ClassroomService : BaseService<Classroom>, IClassroomService
    {
        private IUOW UOW;
        private IClassroomValidator ClassroomValidator;
        private IPermissionService PermissionService;
        public ClassroomService(
            IUOW UOW,
            IClassroomValidator ClassroomValidator,
            IPermissionService PermissionService
        )
        {
            this.UOW = UOW;
            this.ClassroomValidator = ClassroomValidator;
            this.PermissionService = PermissionService;
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
                await AutoAddCreatorInClassAndSetRole(Classroom);
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

        private async Task<Classroom> AutoAddCreatorInClassAndSetRole(Classroom Classroom)
        {
            List<Role> SystemRoles = await UOW.PermissionRepository.ListSystemRole();
            AppUserClassroomMapping AppUserClassroomMapping = new AppUserClassroomMapping();
            AppUserClassroomMapping.AppUserId = PermissionService.GetAppUserId();
            AppUserClassroomMapping.ClassroomId = Classroom.Id;
            AppUserClassroomMapping.RoleId = SystemRoles.Where(x => x.Name == "Teacher").Select(x => x.Id).FirstOrDefault();
            Classroom.AppUserClassroomMappings = new List<AppUserClassroomMapping> { AppUserClassroomMapping };
            return Classroom;
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

        public async Task<bool> Join(string code)
        {
            if (!await ClassroomValidator.Join(code))
                return false;
            try
            {
                List<Classroom> Classrooms = await UOW.ClassroomRepository.List();
                List<string> ClassroomCodes = Classrooms.Select(x => x.Code.Trim()).ToList();
                if (ClassroomCodes.Contains(code.Trim()))
                {
                    List<Role> SystemRoles = await UOW.PermissionRepository.ListSystemRole();
                    Classroom CurrentClassrom = Classrooms.FirstOrDefault(x => x.Code == code);
                    AppUserClassroomMapping AppUserClassroomMapping = new AppUserClassroomMapping();
                    AppUserClassroomMapping.AppUserId = PermissionService.GetAppUserId();
                    AppUserClassroomMapping.ClassroomId = CurrentClassrom.Id;
                    AppUserClassroomMapping.RoleId = SystemRoles.Where(x => x.Name == "Student").Select(x => x.Id).FirstOrDefault();

                    var NewAppUserClassroomMappings = CurrentClassrom.AppUserClassroomMappings.ToList();
                    NewAppUserClassroomMappings.Add(AppUserClassroomMapping);
                    await UOW.ClassroomRepository.BulkMerge(NewAppUserClassroomMappings);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {

            }
            return false;
        }

        private async Task BuildCode(Classroom Classroom)
        {
            Classroom.Code = "C" + Classroom.Id;
            await UOW.ClassroomRepository.UpdateCode(Classroom);
        }
    }
}
