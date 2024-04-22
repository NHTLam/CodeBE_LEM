using CodeBE_LEM.Common;
using CodeBE_LEM.Entities;
using CodeBE_LEM.Repositories;
using CodeBE_LEM.Services.ClassEventService;
using CodeBE_LEM.Entities;

namespace CodeBE_LEM.Services.ClassroomService
{
    public interface IClassEventService
    {
        Task<List<ClassEvent>> List(FilterDTO FilterDTO);
        Task<ClassEvent> Get(long Id);
        Task<ClassEvent> Create(ClassEvent ClassEvent);
        Task<ClassEvent> Update(ClassEvent ClassEvent);
        Task<ClassEvent> Delete(ClassEvent ClassEvent);
        Task<Comment> CreateComment(Comment Comment);
        Task<Comment> UpdateComment(Comment Comment);
        Task<Comment> DeleteComment(Comment Comment);
        Task<Question> CreateQuestion(Question Question);
        Task<Question> UpdateQuestion(Question Question);
        Task<Question> DeleteQuestion(Question Question);
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

        public async Task<Comment> CreateComment(Comment Comment)
        {
            try
            {
                //var ClassEventId = await UOW.ClassEventRepository.Get(Comment.ClassEventId);

                //if (ClassEventId != null)
                //{
                //    await UOW.CommentRepository.Create(Comment);
                //    Comment = await UOW.CommentRepository.Get(Comment.Id);
                //    return Comment;
                //}

            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            return null;
        }
        public async Task<Comment> DeleteComment(Comment Comment)
        {
            try
            {
                Comment = await UOW.CommentRepository.Get(Comment.Id);

                await UOW.CommentRepository.Delete(Comment);
                Comment = await UOW.CommentRepository.Get(Comment.Id);
                return Comment;

            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            return null;
        }
        public async Task<Comment> UpdateComment(Comment Comment)
        {
            try
            {
                var oldData = await UOW.CommentRepository.Get(Comment.Id);

                Comment.ClassEventId = oldData.ClassEventId;
                await UOW.CommentRepository.Update(Comment);

                Comment = await UOW.CommentRepository.Get(Comment.Id);
                return Comment;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        public async Task<Question> CreateQuestion(Question Question)
        {
            try
            {
                var ClassEventId = await UOW.ClassEventRepository.Get(Question.ClassEventId);

                if (ClassEventId != null)
                {
                    await UOW.QuestionRepository.Create(Question);
                    Question = await UOW.QuestionRepository.Get(Question.Id);
                    return Question;
                }

            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            return null;
        }
        public async Task<Question> DeleteQuestion(Question Question)
        {
            try
            {
                Question = await UOW.QuestionRepository.Get(Question.Id);

                await UOW.QuestionRepository.Delete(Question);
                Question = await UOW.QuestionRepository.Get(Question.Id);
                return Question;

            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            return null;
        }
        public async Task<Question> UpdateQuestion(Question Question)
        {
            try
            {
                var oldData = await UOW.QuestionRepository.Get(Question.Id);

                Question.ClassEventId = oldData.ClassEventId;
                await UOW.QuestionRepository.Update(Question);

                Question = await UOW.QuestionRepository.Get(Question.Id);
                return Question;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            return null;
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
                List<ClassEvent> ClassEvents = await UOW.ClassEventRepository.List(FilterDTO.ClassroomId!.Value);

                ClassEvents = FilterData(ClassEvents, FilterDTO);
                if (FilterDTO.Pinned != null)
                {
                    ClassEvents = ClassEvents.Where(x => x.Pinned == FilterDTO.Pinned).ToList();
                }
                if (FilterDTO.IsClassWork != null)
                {
                    ClassEvents = ClassEvents.Where(x => x.IsClassWork == FilterDTO.IsClassWork).ToList();
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
