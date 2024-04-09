using CodeBE_LEM.Entities;
using CodeBE_LEM.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeBE_LEM.Repositories
{
    public interface IQuestionRepository
    {
        Task<List<Question>> List();
        Task<Question> Get(long Id);
        Task<bool> Create(Question Question);
        Task<bool> Update(Question Question);
        Task<bool> Delete(Question Question);
    }

    public class QuestionRepository : IQuestionRepository
    {
        private DataContext DataContext;
        public QuestionRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        public async Task<bool> Create(Question Question)
        {
            QuestionDAO QuestionDAO = new QuestionDAO();
            QuestionDAO.ClassEventId = Question.ClassEventId;
            QuestionDAO.Description = Question.Description;
            QuestionDAO.Name = Question.Name;
            QuestionDAO.QuestionAnswer = Question.QuestionAnswer;
            QuestionDAO.StudentAnswer = Question.StudentAnswer;
            DataContext.Questions.Add(QuestionDAO);
            await DataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Question Question)
        {
            QuestionDAO? QuestionDAO = DataContext.Questions
                .Where(x => x.Id == Question.Id)
                .FirstOrDefault();
            if (QuestionDAO == null)
                return false;
            DataContext.Questions.Remove(QuestionDAO);
            await DataContext.SaveChangesAsync();
            return true;
        }

        public async Task<Question> Get(long Id)
        {
            Question? Question = await DataContext.Questions.AsNoTracking()
                .Where(x => x.Id == Id)
                .Select(x => new Question
                {
                    Id = x.Id,
                    ClassEventId = x.ClassEventId,
                    Description = x.Description,
                    Name = x.Name,
                    QuestionAnswer = x.QuestionAnswer,
                    StudentAnswer = x.StudentAnswer,
                    ClassEvent = new ClassEvent
                    {
                        Id = x.ClassEvent.Id,
                        Name = x.ClassEvent.Name,
                        Code = x.ClassEvent.Code,
                        ClassroomId = x.ClassEvent.ClassroomId,
                        Description = x.ClassEvent.Description,
                        Instruction = x.ClassEvent.Instruction,
                        IsClassWork = x.ClassEvent.IsClassWork,
                        Pinned = x.ClassEvent.Pinned,
                        CreatedAt = x.ClassEvent.CreatedAt,
                        EndAt = x.ClassEvent.EndAt,
                        UpdatedAt = x.ClassEvent.UpdatedAt,
                        DeletedAt = x.ClassEvent.DeletedAt,
                        Classroom = new Classroom
                        {
                            Id = x.ClassEvent.Classroom.Id,
                            Name = x.ClassEvent.Classroom.Name,
                            Code = x.ClassEvent.Classroom.Code,
                            Description = x.ClassEvent.Classroom.Description,
                            CreatedAt = x.ClassEvent.Classroom.CreatedAt,
                            UpdatedAt = x.ClassEvent.Classroom.UpdatedAt,
                            DeletedAt = x.ClassEvent.Classroom.DeletedAt,
                        },
                    },
                }).FirstOrDefaultAsync();

            return Question;
        }

        public async Task<List<Question>> List()
        {
            List<Question> Questions = await DataContext.Questions.AsNoTracking()
            .Select(x => new Question
            {
                Id = x.Id,
                ClassEventId = x.ClassEventId,
                Description = x.Description,
                Name = x.Name,
                QuestionAnswer = x.QuestionAnswer,
                StudentAnswer = x.StudentAnswer,
                ClassEvent = new ClassEvent
                {
                    Id = x.ClassEvent.Id,
                    Name = x.ClassEvent.Name,
                    Code = x.ClassEvent.Code,
                    ClassroomId = x.ClassEvent.ClassroomId,
                    Description = x.ClassEvent.Description,
                    Instruction = x.ClassEvent.Instruction,
                    IsClassWork = x.ClassEvent.IsClassWork,
                    Pinned = x.ClassEvent.Pinned,
                    CreatedAt = x.ClassEvent.CreatedAt,
                    EndAt = x.ClassEvent.EndAt,
                    UpdatedAt = x.ClassEvent.UpdatedAt,
                    DeletedAt = x.ClassEvent.DeletedAt,
                    Classroom = new Classroom
                    {
                        Id = x.ClassEvent.Classroom.Id,
                        Name = x.ClassEvent.Classroom.Name,
                        Code = x.ClassEvent.Classroom.Code,
                        Description = x.ClassEvent.Classroom.Description,
                        CreatedAt = x.ClassEvent.Classroom.CreatedAt,
                        UpdatedAt = x.ClassEvent.Classroom.UpdatedAt,
                        DeletedAt = x.ClassEvent.Classroom.DeletedAt,
                    },
                },
            }).ToListAsync();

            return Questions;
        }

        public async Task<bool> Update(Question Question)
        {
            QuestionDAO? QuestionDAO = DataContext.Questions
                .Where(x => x.Id == Question.Id)
                .FirstOrDefault();
            if (QuestionDAO == null)
                return false;
            QuestionDAO.ClassEventId = Question.ClassEventId;
            QuestionDAO.Description = Question.Description;
            QuestionDAO.Name = Question.Name;
            QuestionDAO.QuestionAnswer = Question.QuestionAnswer;
            QuestionDAO.StudentAnswer = Question.StudentAnswer;
            await DataContext.SaveChangesAsync();
            return true;
        }
    }
}
