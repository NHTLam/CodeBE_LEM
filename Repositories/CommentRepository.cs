using CodeBE_LEM.Entities;
using CodeBE_LEM.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeBE_LEM.Repositories
{
    public interface ICommentRepository
    {
        Task<List<Comment>> List();
        Task<Comment> Get(long Id);
        Task<bool> Create(Comment Comment);
        Task<bool> Update(Comment Comment);
        Task<bool> Delete(Comment Comment);
    }
    public class CommentRepository : ICommentRepository
    {
        private DataContext DataContext;
        public CommentRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        public async Task<bool> Create(Comment Comment)
        {
            CommentDAO CommentDAO = new CommentDAO();
            CommentDAO.ClassEventId = Comment.ClassEventId;
            CommentDAO.Description = Comment.Description;
            DataContext.Comments.Add(CommentDAO);
            await DataContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Comment Comment)
        {
            CommentDAO? CommentDAO = DataContext.Comments
                .Where(x => x.Id == Comment.Id)
                .FirstOrDefault();
            if (CommentDAO == null)
                return false;
            DataContext.Comments.Remove(CommentDAO);
            await DataContext.SaveChangesAsync();
            return true;
        }

        public async Task<Comment> Get(long Id)
        {
            Comment? Comment = await DataContext.Comments.AsNoTracking()
                .Where(x => x.Id == Id)
                .Select(x => new Comment
                {
                    Id = x.Id,
                    ClassEventId = x.ClassEventId,
                    Description = x.Description,
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

            return Comment;
        }

        public async Task<List<Comment>> List()
        {
            List<Comment> Comments = await DataContext.Comments.AsNoTracking()
            .Select(x => new Comment
            {
                Id = x.Id,
                ClassEventId = x.ClassEventId,
                Description = x.Description,
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

            return Comments;
        }

        public async Task<bool> Update(Comment Comment)
        {
            CommentDAO? CommentDAO = DataContext.Comments
                .Where(x => x.Id == Comment.Id)
                .FirstOrDefault();
            if (CommentDAO == null)
                return false;
            CommentDAO.ClassEventId = Comment.ClassEventId;
            CommentDAO.Description = Comment.Description;
            await DataContext.SaveChangesAsync();
            return true;
        }

    }
}
