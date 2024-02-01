using HikeBE.Models;

namespace HikeBE.Repositories
{
    public interface IUOW
    {
        IHikeRepository HikeRepository { get; }
        IImgRepository ImgRepository { get; }
        IObservationRepository ObservationRepository { get; }
        IAppUserRepository AppUserRepository { get; }
    }
    public class UOW : IUOW
    {
        private DataContext DataContext;
        public IHikeRepository HikeRepository { get; private set; }
        public IImgRepository ImgRepository { get; private set; }
        public IObservationRepository ObservationRepository { get; private set; }
        public IAppUserRepository AppUserRepository { get; private set; }

        public UOW(DataContext DataContext)
        {
            this.DataContext = DataContext;
            HikeRepository = new HikeRepository(DataContext);
            ImgRepository = new ImgRepository(DataContext);
            ObservationRepository = new ObservationRepository(DataContext);
            AppUserRepository = new AppUserRepository(DataContext);
        }
    }
}
