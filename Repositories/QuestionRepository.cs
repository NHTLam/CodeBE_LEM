using CodeBE_LEM.Entities;
using CodeBE_LEM.Models;
using CodeBE_LEM.Entities;
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
            QuestionDAO.Instruction = Question.Instruction;
            QuestionDAO.Name = Question.Name;
            QuestionDAO.CorrectAnswer = Question.CorrectAnswer;
            DataContext.Questions.Add(QuestionDAO);
            await DataContext.SaveChangesAsync();
            Question.Id = QuestionDAO.Id;
            await SaveReference(Question);
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
                    CorrectAnswer = x.CorrectAnswer,
                    ClassEvent = x.ClassEvent == null ? null : new ClassEvent
                    {
                        Id = x.ClassEvent.Id,
                        Name = x.ClassEvent.Name,
                        Code = x.ClassEvent.Code,
                        ClassroomId = x.ClassEvent.ClassroomId,
                        Description = x.ClassEvent.Description,
                        IsClassWork = x.ClassEvent.IsClassWork,
                        Pinned = x.ClassEvent.Pinned,
                        CreatedAt = x.ClassEvent.CreatedAt,
                        EndAt = x.ClassEvent.EndAt,
                        UpdatedAt = x.ClassEvent.UpdatedAt,
                        DeletedAt = x.ClassEvent.DeletedAt,
                        Classroom = x.ClassEvent.Classroom == null ? null : new Classroom
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
                CorrectAnswer = x.CorrectAnswer,
                ClassEvent = x.ClassEvent == null ? null :new ClassEvent
                {
                    Id = x.ClassEvent.Id,
                    Name = x.ClassEvent.Name,
                    Code = x.ClassEvent.Code,
                    ClassroomId = x.ClassEvent.ClassroomId,
                    Description = x.ClassEvent.Description,
                    IsClassWork = x.ClassEvent.IsClassWork,
                    Pinned = x.ClassEvent.Pinned,
                    CreatedAt = x.ClassEvent.CreatedAt,
                    EndAt = x.ClassEvent.EndAt,
                    UpdatedAt = x.ClassEvent.UpdatedAt,
                    DeletedAt = x.ClassEvent.DeletedAt,
                    Classroom = x.ClassEvent.Classroom == null ? null : new Classroom
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
            QuestionDAO.Instruction = Question.Instruction;
            QuestionDAO.Description = Question.Description;
            QuestionDAO.Name = Question.Name;
            QuestionDAO.CorrectAnswer = Question.CorrectAnswer;
            await DataContext.SaveChangesAsync();
            await SaveReference(Question);
            return true;
        }

        private async Task SaveReference(Question Question)
        {

            if (Question.Answers == null || Question.Answers.Count == 0)
            {
                await DataContext.Answers
                    .Where(x => x.QuestionId == Question.Id)
                    .DeleteFromQueryAsync();
            }
            else
            {
                var AnswerIds = Question.Answers.Select(x => x.Id).Distinct().ToList();
                await DataContext.Answers
                    .Where(x => x.QuestionId == Question.Id)
                    .Where(x => !AnswerIds.Contains(x.Id))
                    .DeleteFromQueryAsync();

                List<AnswerDAO> AnswerDAOs = new List<AnswerDAO>();
                foreach (Answer Answer in Question.Answers)
                {
                    AnswerDAO AnswerDAO = new AnswerDAO();
                    AnswerDAO.Id = Answer.Id;
                    AnswerDAO.Code = Answer.Code;
                    AnswerDAO.Name = Answer.Name;
                    AnswerDAO.QuestionId = Question.Id;

                    AnswerDAOs.Add(AnswerDAO);
                }
                await DataContext.BulkMergeAsync(AnswerDAOs);
            }

            if (Question.StudentAnswers == null || Question.StudentAnswers.Count == 0)
            {
                await DataContext.StudentAnswers
                    .Where(x => x.QuestionId == Question.Id)
                    .DeleteFromQueryAsync();
            }
            else
            {
                var StudentAnswersIds = Question.StudentAnswers.Select(x => x.Id).Distinct().ToList();
                await DataContext.StudentAnswers
                    .Where(x => x.QuestionId == Question.Id)
                    .Where(x => !StudentAnswersIds.Contains(x.Id))
                    .DeleteFromQueryAsync();

                List<StudentAnswerDAO> StudentAnswerDAOs = new List<StudentAnswerDAO>();
                foreach (StudentAnswer StudentAnswer in Question.StudentAnswers)
                {
                    StudentAnswerDAO StudentAnswerDAO = new StudentAnswerDAO();
                    StudentAnswerDAO.Id = StudentAnswer.Id;
                    StudentAnswerDAO.AppUserId = StudentAnswer.AppUserId;
                    StudentAnswerDAO.Name = StudentAnswer.Name;
                    StudentAnswerDAO.AppUserFeedbackId = StudentAnswer.AppUserFeedbackId;
                    StudentAnswerDAO.Grade = StudentAnswer.Grade;
                    StudentAnswerDAO.GradeAt = StudentAnswer.GradeAt;
                    StudentAnswerDAO.Feedback = StudentAnswer.Feedback;
                    StudentAnswerDAO.SubmitAt = StudentAnswer.SubmitAt;
                    StudentAnswerDAO.QuestionId = Question.Id;

                    StudentAnswerDAOs.Add(StudentAnswerDAO);
                }
                await DataContext.BulkMergeAsync(StudentAnswerDAOs);
            }
        }
    }
}
