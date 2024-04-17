using CodeBE_LEM.Entities;
using CodeBE_LEM.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CodeBE_LEM.Repositories
{
    public interface IClassEventRepository
    {
        Task<List<ClassEvent>> List(long Id);
        Task<ClassEvent> Get(long Id);
        Task<bool> Create(ClassEvent ClassEvent);
        Task<bool> Update(ClassEvent ClassEvent);
        Task<bool> Delete(ClassEvent ClassEvent);
        Task<bool> UpdateCode(ClassEvent ClassEvent);
    }

    public class ClassEventRepository : IClassEventRepository
    {
        private DataContext DataContext;
        public ClassEventRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        public async Task<bool> Create(ClassEvent ClassEvent)
        {
            ClassEventDAO ClassEventDAO = new ClassEventDAO();
            ClassEventDAO.ClassroomId = ClassEvent.ClassroomId;
            ClassEventDAO.Code = ClassEvent.Code;
            ClassEventDAO.Name = ClassEvent.Name;
            ClassEventDAO.IsClassWork = ClassEvent.IsClassWork;
            ClassEventDAO.Description = ClassEvent.Description;
            ClassEventDAO.Pinned = ClassEvent.Pinned;
            ClassEventDAO.StartAt = ClassEvent.StartAt;
            ClassEventDAO.CreatedAt = ClassEvent.CreatedAt;
            ClassEventDAO.EndAt = ClassEvent.EndAt;
            ClassEventDAO.UpdatedAt = ClassEvent.UpdatedAt;
            ClassEventDAO.DeletedAt = ClassEvent.DeletedAt;
            DataContext.ClassEvents.Add(ClassEventDAO);
            await DataContext.SaveChangesAsync();
            ClassEvent.Id = ClassEventDAO.Id;
            await SaveReference(ClassEvent);
            return true;
        }

        public async Task<bool> Delete(ClassEvent ClassEvent)
        {
            ClassEventDAO? ClassEventDAO = DataContext.ClassEvents
                .Where(x => x.Id == ClassEvent.Id)
                .FirstOrDefault();
            if (ClassEventDAO == null)
                return false;
            ClassEventDAO.DeletedAt = DateTime.Now;
            await DataContext.SaveChangesAsync();
            await SaveReference(ClassEvent);
            return true;
        }

        public async Task<ClassEvent> Get(long Id)
        {
            ClassEvent? ClassEvent = await DataContext.ClassEvents.AsNoTracking()
                .Where(x => x.DeletedAt == null)
                .Where(x => x.Id == Id)
                .Select(x => new ClassEvent
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    ClassroomId = x.ClassroomId,
                    Description = x.Description,
                    StartAt = x.StartAt,
                    IsClassWork = x.IsClassWork,
                    Pinned = x.Pinned,
                    CreatedAt = x.CreatedAt,
                    EndAt = x.EndAt,
                    UpdatedAt = x.UpdatedAt,
                    DeletedAt = x.DeletedAt,
                    Classroom = new Classroom
                    {
                        Id = x.Classroom.Id,
                        Name = x.Classroom.Name,
                        Code = x.Classroom.Code,
                        Description = x.Classroom.Description,
                        CreatedAt = x.Classroom.CreatedAt,
                        UpdatedAt = x.Classroom.UpdatedAt,
                        DeletedAt = x.Classroom.DeletedAt,
                    },
                }).FirstOrDefaultAsync();

            if (ClassEvent == null)
                return null;

            ClassEvent.Comments =  await DataContext.Comments.AsNoTracking()
                .Where(x => x.ClassEventId == ClassEvent.Id)
                .Select(x => new Comment
                {
                    Id = x.Id,
                    ClassEventId = x.ClassEventId,
                    Description = x.Description,

                }).ToListAsync();

            ClassEvent.Questions = await DataContext.Questions.AsNoTracking()
                .Where(x => x.ClassEventId == ClassEvent.Id)
                .Select(x => new Question
                {
                    Id = x.Id,
                    ClassEventId = x.ClassEventId,
                    Description = x.Description,
                    Instruction = x.Instruction,
                    Name = x.Name,
                    CorrectAnswer = x.CorrectAnswer,
                    StudentAnswer = x.StudentAnswer,

                }).ToListAsync();

            return ClassEvent;
        }

        public async Task<List<ClassEvent>> List(long Id)
        {
            List<ClassEvent> ClassEvents = await DataContext.ClassEvents.AsNoTracking()
            .Where(x => x.DeletedAt == null)
            .Where(x => x.ClassroomId == Id)
            .Select(x => new ClassEvent
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                ClassroomId = x.ClassroomId,
                Description = x.Description,
                IsClassWork = x.IsClassWork,
                StartAt = x.StartAt,
                Pinned = x.Pinned,
                CreatedAt = x.CreatedAt,
                EndAt = x.EndAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
                Classroom = new Classroom
                {
                    Id = x.Classroom.Id,
                    Name = x.Classroom.Name,
                    Code = x.Classroom.Code,
                    Description = x.Classroom.Description,
                    CreatedAt = x.Classroom.CreatedAt,
                    UpdatedAt = x.Classroom.UpdatedAt,
                    DeletedAt = x.Classroom.DeletedAt,
                },
            }).ToListAsync();

            List<Comment> Comments = await DataContext.Comments.AsNoTracking()
                .Select(x => new Comment
                {
                    Id = x.Id,
                    ClassEventId = x.ClassEventId,
                    Description = x.Description,

                }).ToListAsync();

            List<Question> Questions = await DataContext.Questions.AsNoTracking()
                .Select(x => new Question
                {
                    Id = x.Id,
                    ClassEventId = x.ClassEventId,
                    Instruction = x.Instruction,
                    Description = x.Description,
                    Name = x.Name,
                    CorrectAnswer = x.CorrectAnswer,
                    StudentAnswer = x.StudentAnswer,

                }).ToListAsync();

            foreach (ClassEvent ClassEvent in ClassEvents)
            {
                ClassEvent.Comments = Comments
                    .Where(x => x.ClassEventId == ClassEvent.Id)
                    .ToList();

                ClassEvent.Questions = Questions
                    .Where(x => x.ClassEventId == ClassEvent.Id)
                    .ToList();
            }

            return ClassEvents;
        }

        public async Task<bool> Update(ClassEvent ClassEvent)
        {
            ClassEventDAO? ClassEventDAO = DataContext.ClassEvents
                .Where(x => x.Id == ClassEvent.Id)
                .FirstOrDefault();
            if (ClassEventDAO == null)
                return false;
            ClassEventDAO.ClassroomId = ClassEvent.ClassroomId;
            ClassEventDAO.Code = ClassEvent.Code;
            ClassEventDAO.Name = ClassEvent.Name;
            ClassEventDAO.IsClassWork = ClassEvent.IsClassWork;
            ClassEventDAO.Description = ClassEvent.Description;
            ClassEventDAO.Pinned = ClassEvent.Pinned;
            ClassEventDAO.StartAt = ClassEvent.StartAt;
            ClassEventDAO.CreatedAt = ClassEvent.CreatedAt;
            ClassEventDAO.EndAt = ClassEvent.EndAt;
            ClassEventDAO.UpdatedAt = ClassEvent.UpdatedAt;
            ClassEventDAO.DeletedAt = ClassEvent.DeletedAt;
            await DataContext.SaveChangesAsync();
            await SaveReference(ClassEvent);
            return true;
        }

        public async Task<bool> UpdateCode(ClassEvent ClassEvent)
        {
            ClassEventDAO? ClassEventDAO = DataContext.ClassEvents
                .Where(x => x.Id == ClassEvent.Id)
                .FirstOrDefault();
            if (ClassEventDAO == null)
                return false;
            ClassEventDAO.Id = ClassEvent.Id;
            ClassEventDAO.Code = ClassEvent.Code;
            await DataContext.SaveChangesAsync();
            return true;
        }

        private async Task SaveReference(ClassEvent ClassEvent)
        {
            if (ClassEvent.Comments == null || ClassEvent.Comments.Count == 0)
            {
                await DataContext.Comments
                    .Where(x => x.ClassEventId == ClassEvent.Id)
                    .DeleteFromQueryAsync();
            }
            else
            {
                var CommentIds = ClassEvent.Comments.Select(x => x.Id).Distinct().ToList();
                await DataContext.Comments
                    .Where(x => x.ClassEventId == ClassEvent.Id)
                    .Where(x => !CommentIds.Contains(x.Id))
                    .DeleteFromQueryAsync();

                List<CommentDAO> CommentDAOs = new List<CommentDAO>();
                foreach (Comment Comment in ClassEvent.Comments)
                {
                    CommentDAO CommentDAO = new CommentDAO();
                    CommentDAO.Id = Comment.Id;
                    CommentDAO.ClassEventId = ClassEvent.Id;
                    CommentDAO.Description = Comment.Description;
                    CommentDAOs.Add(CommentDAO);
                }
                await DataContext.BulkMergeAsync(CommentDAOs);
            }

            if (ClassEvent.Questions == null || ClassEvent.Questions.Count == 0)
            {
                await DataContext.Questions
                    .Where(x => x.ClassEventId == ClassEvent.Id)
                    .DeleteFromQueryAsync();
            }
            else
            {
                var QuestionIds = ClassEvent.Questions.Select(x => x.Id).Distinct().ToList();
                await DataContext.Questions
                    .Where(x => x.ClassEventId == ClassEvent.Id)
                    .Where(x => !QuestionIds.Contains(x.Id))
                    .DeleteFromQueryAsync();

                List<QuestionDAO> QuestionDAOs = new List<QuestionDAO>();
                foreach (Question Question in ClassEvent.Questions)
                {
                    QuestionDAO QuestionDAO = new QuestionDAO();
                    QuestionDAO.Id = Question.Id;
                    QuestionDAO.ClassEventId = ClassEvent.Id;
                    QuestionDAO.Name = Question.Name;
                    QuestionDAO.CorrectAnswer = Question.CorrectAnswer;
                    QuestionDAO.StudentAnswer = Question.StudentAnswer;
                    QuestionDAO.Description = Question.Description;
                    QuestionDAOs.Add(QuestionDAO);
                }
                await DataContext.BulkMergeAsync(QuestionDAOs);
            }
        }
    }
}
