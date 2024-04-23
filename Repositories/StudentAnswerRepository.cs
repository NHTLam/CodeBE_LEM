using CodeBE_LEM.Entities;
using CodeBE_LEM.Entities;
using CodeBE_LEM.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeBE_LEM.Repositories
{
    public interface IStudentAnswerRepository
    {
        Task<List<StudentAnswer>> List(List<long> Ids);
        Task<List<StudentAnswer>> Detail(long Id);
        Task<StudentAnswer> Get(long Id);
        Task<bool> Create(StudentAnswer StudentAnswer);
        Task<bool> Update(StudentAnswer StudentAnswer);
        Task<bool> Delete(StudentAnswer StudentAnswer);
    }

    public class StudentAnswerRepository : IStudentAnswerRepository
    {
        private DataContext DataContext;
        public StudentAnswerRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        public async Task<bool> Create(StudentAnswer StudentAnswer)
        {
            StudentAnswerDAO StudentAnswerDAO = new StudentAnswerDAO();
            StudentAnswerDAO.Name = StudentAnswer.Name;
            StudentAnswerDAO.QuestionId = StudentAnswer.QuestionId;
            StudentAnswerDAO.AppUserId = StudentAnswer.AppUserId;
            StudentAnswerDAO.SubmitAt = DateTime.Now;
            DataContext.StudentAnswers.Add(StudentAnswerDAO);
            await DataContext.SaveChangesAsync();
            StudentAnswer.Id = StudentAnswerDAO.Id;
            return true;
        }

        public async Task<bool> Delete(StudentAnswer StudentAnswer)
        {
            StudentAnswerDAO? StudentAnswerDAO = DataContext.StudentAnswers
                .Where(x => x.Id == StudentAnswer.Id)
                .FirstOrDefault();
            if (StudentAnswerDAO == null)
                return false;
            DataContext.StudentAnswers.Remove(StudentAnswerDAO);
            await DataContext.SaveChangesAsync();
            return true;
        }

        public async Task<StudentAnswer> Get(long Id)
        {
            StudentAnswer? StudentAnswer = await DataContext.StudentAnswers.AsNoTracking()
                .Where(x => x.Id == Id)
                .Select(x => new StudentAnswer
                {
                    Id = x.Id,
                    Name = x.Name,
                    QuestionId = x.QuestionId,
                    AppUserId = x.AppUserId,
                    Grade = x.Grade,
                    AppUserFeedbackId = x.AppUserFeedbackId,
                    GradeAt = x.GradeAt,
                    Feedback = x.Feedback,
                    SubmitAt = x.SubmitAt,
                    Question = x.Question == null ? null : new Question
                    {
                        Id = x.Question.Id,
                        Name = x.Question.Name,
                        ClassEventId = x.Question.ClassEventId,
                        Description = x.Question.Description,
                    },
                    AppUser = x.AppUser == null ? null : new AppUser
                    {
                        Id = x.AppUser.Id,
                        UserName = x.AppUser.UserName
                    },
                    AppUserFeedback = x.AppUserFeedback == null ? null : new AppUser
                    {
                        Id = x.AppUserFeedback.Id,
                        UserName = x.AppUserFeedback.UserName
                    }
                }).FirstOrDefaultAsync();

            return StudentAnswer;
        }

        public async Task<List<StudentAnswer>> List(List<long> Ids)
        {
            List<StudentAnswer> StudentAnswers = await DataContext.StudentAnswers.AsNoTracking()
            .Where (x => Ids.Contains(x.QuestionId))
            .Select(x => new StudentAnswer
            {
                Id = x.Id,
                Name = x.Name,
                QuestionId = x.QuestionId,
                AppUserId = x.AppUserId,
                Grade = x.Grade,
                AppUserFeedbackId = x.AppUserFeedbackId,
                GradeAt = x.GradeAt,
                Feedback = x.Feedback,
                SubmitAt = x.SubmitAt,
                Question = x.Question == null ? null : new Question
                {
                    Id = x.Question.Id,
                    Name = x.Question.Name,
                    ClassEventId = x.Question.ClassEventId,
                    Description = x.Question.Description,
                },
                AppUser = new AppUser
                {
                    Id = x.AppUser.Id,
                    UserName = x.AppUser.UserName
                },
                AppUserFeedback = x.AppUserFeedback == null ? null : new AppUser
                {
                    Id = x.AppUserFeedback.Id,
                    UserName = x.AppUserFeedback.UserName
                }
            }).ToListAsync();

            return StudentAnswers;
        }

        public async Task<List<StudentAnswer>> Detail(long Id)
        {
            List<StudentAnswer> test = await DataContext.StudentAnswers.AsNoTracking()
            .Where(x => x.AppUserId == Id).Select(x => new StudentAnswer()).ToListAsync();

            List<StudentAnswer> StudentAnswers = await DataContext.StudentAnswers.AsNoTracking()
            .Where(x => x.AppUserId == Id)
            .Select(x => new StudentAnswer
            {
                Id = x.Id,
                Name = x.Name,
                QuestionId = x.QuestionId,
                AppUserId = x.AppUserId,
                Grade = x.Grade,
                AppUserFeedbackId = x.AppUserFeedbackId,
                GradeAt = x.GradeAt,
                Feedback = x.Feedback,
                SubmitAt = x.SubmitAt,
                Question = x.Question == null ? null : new Question
                {
                    Id = x.Question.Id,
                    Name = x.Question.Name,
                    ClassEventId = x.Question.ClassEventId,
                    Description = x.Question.Description,
                },
                AppUser = x.AppUser == null ? null : new AppUser
                {
                    Id = x.AppUser.Id,
                    UserName = x.AppUser.UserName
                },
                AppUserFeedback = x.AppUserFeedback == null ? null : new AppUser
                {
                    Id = x.AppUserFeedback.Id,
                    UserName = x.AppUserFeedback.UserName
                }
            }).ToListAsync();

            return StudentAnswers;
        }

        public async Task<bool> Update(StudentAnswer StudentAnswer)
        {
            StudentAnswerDAO? StudentAnswerDAO = DataContext.StudentAnswers
                .Where(x => x.Id == StudentAnswer.Id)
                .FirstOrDefault();
            if (StudentAnswerDAO == null)
                return false;
            StudentAnswerDAO.AppUserFeedbackId = StudentAnswer.AppUserFeedbackId;
            StudentAnswerDAO.Grade = StudentAnswer.Grade;
            StudentAnswerDAO.GradeAt = DateTime.Now;
            StudentAnswerDAO.Feedback = StudentAnswer.Feedback;
            await DataContext.SaveChangesAsync();
            return true;
        }
    }
}
