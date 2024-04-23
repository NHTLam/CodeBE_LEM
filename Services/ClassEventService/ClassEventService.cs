using CodeBE_LEM.Common;
using CodeBE_LEM.Entities;
using CodeBE_LEM.Repositories;
using CodeBE_LEM.Services.ClassEventService;
using CodeBE_LEM.Services;
using CodeBE_LEM.Common;
using CodeBE_LEM.Entities;
using CodeBE_LEM.Repositories;
using CodeBE_LEM.Services.ClassEventService;
using System.Linq;

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
        Task<StudentAnswer> CreateStudentAnswer(StudentAnswer StudentAnswer);
        Task<List<StudentAnswer>> DetailStudentAnswer(StudentAnswer StudentAnswer);
        Task<List<AppUser>> ListStudentAnswer(StudentAnswer StudentAnswer);
        Task<StudentAnswer> UpdateStudentAnswer(StudentAnswer StudentAnswer);
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
                await UOW.CommentRepository.Create(Comment);
                Comment = await UOW.CommentRepository.Get(Comment.Id);
                return Comment;

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

        public async Task<List<StudentAnswer>> DetailStudentAnswer(StudentAnswer StudentAnswer)
        {
            try
            {
                List<StudentAnswer> StudentAnswers = await UOW.StudentAnswerRepository.Detail(StudentAnswer.AppUserId);

                StudentAnswers = StudentAnswers.Where(x => x.Question?.ClassEventId == StudentAnswer.ClassEventId).ToList();

                return StudentAnswers;

            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            return null;
        }

        public async Task<List<AppUser>> ListStudentAnswer(StudentAnswer StudentAnswer)
        {
            try
            {
                ClassEvent ClassEvent = await UOW.ClassEventRepository.Get(StudentAnswer.ClassEventId.Value);

                var QuestionIds = ClassEvent?.Questions?.Select(x => x.Id).ToList();

                if (QuestionIds != null && QuestionIds.Count != 0)
                {
                    List<StudentAnswer> StudentAnswers = await UOW.StudentAnswerRepository.List(QuestionIds);
                    var AppUsers = StudentAnswers.Select(x => x.AppUser).GroupBy(p => p.Id).Select(g => g.First()).ToList();


                    return AppUsers;
                }


            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            return null;
        }
        public async Task<StudentAnswer> CreateStudentAnswer(StudentAnswer StudentAnswer)
        {
            try
            {
                var QuestionId = await UOW.QuestionRepository.Get(StudentAnswer.QuestionId);

                if (QuestionId != null)
                {
                    StudentAnswer.SubmitAt = DateTime.Now;
                    await UOW.StudentAnswerRepository.Create(StudentAnswer);
                    StudentAnswer = await UOW.StudentAnswerRepository.Get(StudentAnswer.Id);
                    return StudentAnswer;
                }

            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            return null;
        }
        public async Task<StudentAnswer> UpdateStudentAnswer(StudentAnswer StudentAnswer)
        {
            try
            {
                var oldData = await UOW.StudentAnswerRepository.Get(StudentAnswer.Id);

                await UOW.StudentAnswerRepository.Update(StudentAnswer);

                StudentAnswer = await UOW.StudentAnswerRepository.Get(StudentAnswer.Id);
                return StudentAnswer;
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

            if (ClassEvent.Questions != null && ClassEvent.Questions.Count > 0)
            {
                Question QuestionInDb = ClassEvent.Questions.FirstOrDefault();

                if (QuestionInDb != null)
                {
                    var QuestionIds = new List<long>();
                    QuestionIds.Add(QuestionInDb.Id);
                    List<StudentAnswer> StudentAnswers = await UOW.StudentAnswerRepository.List(QuestionIds);

                    foreach (Question Question in ClassEvent.Questions)
                    {
                        Question.StudentAnswers = StudentAnswers;

                    }
                }
            }

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

                if (FilterDTO.Pinned != null)
                {
                    ClassEvents = ClassEvents.Where(x => x.Pinned == FilterDTO.Pinned).ToList();
                }
                if (FilterDTO.IsClassWork != null)
                {
                    ClassEvents = ClassEvents.Where(x => x.IsClassWork == FilterDTO.IsClassWork).ToList();
                }
                if (FilterDTO.AppUserId != null)
                {
                    List<StudentAnswer> StudentAnswers = await UOW.StudentAnswerRepository.Detail(FilterDTO.AppUserId.Value);
                    List<Question> Questions = StudentAnswers.Select(x => x.Question).ToList();
                    var ClassEventIds = Questions.Select(x => x.ClassEventId).Distinct().ToList();

                    foreach (ClassEvent ClassEvent in ClassEvents)
                    {
                        if (ClassEventIds.Contains(ClassEvent.Id))
                        {
                            ClassEvent.IsSubmit = true;
                        }
                    }
                    ClassEvents = FilterData(ClassEvents, FilterDTO);

                }

                return ClassEvents;
            }
            catch (Exception ex)
            {
                throw new Exception();
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
                ClassEvent.AppUserId = oldData.AppUserId;
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
