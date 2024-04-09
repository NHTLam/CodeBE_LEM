using CodeBE_LEM.Common;
using CodeBE_LEM.Entities;
using CodeBE_LEM.Repositories;
using CodeBE_LEM.Services.ClassEventService;

namespace CodeBE_LEM.Services.ClassroomService
{
    public interface IClassEventService
    {
        Task<List<ClassEvent>> List(FilterDTO FilterDTO);
        Task<ClassEvent> Get(long Id);
        Task<ClassEvent> Create(ClassEvent ClassEvent);
        Task<ClassEvent> Update(ClassEvent ClassEvent);
        Task<ClassEvent> Delete(ClassEvent ClassEvent);
    }
    public class ClassEventService : BaseService<ClassEvent>, IClassEventService
    {
        private IUOW UOW;
        private IClassEventValidator ClassEventValidator;
        public ClassEventService(
            IUOW UOW,
            IClassEventValidator ClassEventValidator
        )
        {
            this.UOW = UOW;
            this.ClassEventValidator = ClassEventValidator;
        }
        public async Task<ClassEvent> Create(ClassEvent ClassEvent)
        {
            if (!await ClassEventValidator.Create(ClassEvent))
                return ClassEvent;

            try
            {
                var ClassroomId = await UOW.ClassroomRepository.Get(ClassEvent.ClassroomId);

                if (ClassroomId != null)
                {
                    ClassEvent.Code = string.Empty;
                    ClassEvent.CreatedAt = DateTime.Now;
                    ClassEvent.UpdatedAt = DateTime.Now;
                    ClassEvent.DeletedAt = null;
                    await UOW.ClassEventRepository.Create(ClassEvent);
                    await BuildCodeClassEvent(ClassEvent);
                    ClassEvent = await UOW.ClassEventRepository.Get(ClassEvent.Id);
                    return ClassEvent;
                }
                    
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            return null;
        }

        public async Task<ClassEvent> Delete(ClassEvent ClassEvent)
        {
            if (!await ClassEventValidator.Delete(ClassEvent))
                return ClassEvent;

            try
            {
                ClassEvent = await Get(ClassEvent.Id);
                await UOW.ClassEventRepository.Delete(ClassEvent);
                return ClassEvent;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<ClassEvent> Get(long Id)
        {
            ClassEvent ClassEvent = await UOW.ClassEventRepository.Get(Id);
            if (ClassEvent == null)
                return null;
            await ClassEventValidator.Get(ClassEvent);
            return ClassEvent;
        }

        public async Task<List<ClassEvent>> List(FilterDTO FilterDTO)
        {
            try
            {
                List<ClassEvent> ClassEvents = await UOW.ClassEventRepository.List(1);

                ClassEvents = FilterData(ClassEvents, FilterDTO);
                if (FilterDTO.Pinned != null)
                {
                    ClassEvents = ClassEvents.Where(x => x.Pinned == FilterDTO.Pinned).ToList();
                }
                if (FilterDTO.IsNotification != null)
                {
                    ClassEvents = ClassEvents.Where(x => x.IsClassWork == FilterDTO.IsNotification).ToList();
                }

                return ClassEvents;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<ClassEvent> Update(ClassEvent ClassEvent)
        {
            if (!await ClassEventValidator.Update(ClassEvent))
                return ClassEvent;
            try
            {
                var oldData = await UOW.ClassEventRepository.Get(ClassEvent.Id);
                ClassEvent.CreatedAt = oldData.CreatedAt;
                ClassEvent.UpdatedAt = DateTime.Now;
                ClassEvent.DeletedAt = null;
                ClassEvent.ClassroomId = oldData.ClassroomId;
                await UOW.ClassEventRepository.Update(ClassEvent);
                await BuildCodeClassEvent(ClassEvent);
                ClassEvent = await UOW.ClassEventRepository.Get(ClassEvent.Id);
                return ClassEvent;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        private async Task BuildCodeClassEvent(ClassEvent ClassEvent)
        {
            ClassEvent.Code = "CE" + ClassEvent.Id;
            await UOW.ClassEventRepository.UpdateCode(ClassEvent);
        }
    }
}
