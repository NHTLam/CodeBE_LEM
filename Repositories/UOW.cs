using CodeBE_LEM.Models;

namespace CodeBE_LEM.Repositories
{
    public interface IUOW
    {
        IBoardRepository BoardRepository { get; }
        IJobRepository JobRepository { get; }
        IAppUserRepository AppUserRepository { get; }
        IPermissionRepository PermissionRepository { get; }
        IClassroomRepository ClassroomRepository { get; }
        IClassEventRepository ClassEventRepository { get; }
        ICommentRepository CommentRepository { get; }
        IQuestionRepository QuestionRepository { get; }
        IAttachmentRepository AttachmentRepository { get; }
    }
    public class UOW : IUOW
    {
        private DataContext DataContext;
        public IBoardRepository BoardRepository { get; private set; }
        public IJobRepository JobRepository { get; private set; }
        public IAppUserRepository AppUserRepository { get; private set; }
        public IPermissionRepository PermissionRepository { get; private set; }
        public IClassroomRepository ClassroomRepository { get; private set; }
        public IClassEventRepository ClassEventRepository { get; private set; }
        public ICommentRepository CommentRepository { get; private set; }
        public IQuestionRepository QuestionRepository { get; private set; }
        public IAttachmentRepository AttachmentRepository { get; private set; }

        public UOW(DataContext DataContext)
        {
            this.DataContext = DataContext;
            this.BoardRepository = new BoardRepository(DataContext);
            this.JobRepository = new JobRepository(DataContext);
            this.AppUserRepository = new AppUserRepository(DataContext);
            this.PermissionRepository = new PermissionRepository(DataContext);
            this.ClassroomRepository = new ClassroomRepository(DataContext);
            this.ClassEventRepository = new ClassEventRepository(DataContext);
            this.CommentRepository = new CommentRepository(DataContext);
            this.QuestionRepository = new QuestionRepository(DataContext);
            this.AttachmentRepository = new AttachmentRepository(DataContext);
        }
    }
}
