using HikeBE.Entities;
using HikeBE.Repositories;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace HikeBE.Services.SHike
{
    public interface IHikeService
    {
        Task<List<Hike>> List();
        Task<Hike> Get(long Id);
        Task<Hike> Create(Hike Hike);
        Task<Hike> Update(Hike Hike);
        Task<Hike> Delete(Hike Hike);
        Task<bool> GetDataFromMobile(List<Hike> Hikes);
    }
    public class HikeService : IHikeService
    {
        private IUOW UOW;

        private IHikeValidator HikeValidator;
        public HikeService(
            IUOW UOW,
            IHikeValidator HikeValidator
        )
        {
            this.UOW = UOW;
            this.HikeValidator = HikeValidator;
        }
        public async Task<Hike> Create(Hike Hike)
        {
            if (!await HikeValidator.Create(Hike))
                return Hike;

            try
            {
                await UOW.HikeRepository.Create(Hike);
                Hike = await UOW.HikeRepository.Get(Hike.Id);
                return Hike;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<Hike> Delete(Hike Hike)
        {
            if (!await HikeValidator.Delete(Hike))
                return Hike;

            try
            {
                Hike = await Get(Hike.Id);
                await UOW.HikeRepository.Delete(Hike);
                return Hike;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public async Task<Hike> Get(long Id)
        {
            Hike Hike = await UOW.HikeRepository.Get(Id);
            if (Hike == null)
                return null;
            await HikeValidator.Get(Hike);
            return Hike;
        }

        public async Task<List<Hike>> List()
        {
            try
            {
                List<Hike> Hikes = await UOW.HikeRepository.List();
                return Hikes;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public async Task<Hike> Update(Hike Hike)
        {
            if (!await HikeValidator.Update(Hike))
                return Hike;
            try
            {
                var oldData = await UOW.HikeRepository.Get(Hike.Id);

                await UOW.HikeRepository.Update(Hike);

                Hike = await UOW.HikeRepository.Get(Hike.Id);
                return Hike;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public async Task<bool> GetDataFromMobile(List<Hike> Hikes)
        {
            //if (!await HikeValidator.Create(Hike))
            //    return Hike;

            try
            {
                bool isGetDataSuccess = false;
                if (Hikes != null && Hikes.Count() != 0)
                {
                    #region fetch data
                    var HikeDbs = await UOW.HikeRepository.List();
                    var HikeDbMobileIds = HikeDbs.Where(h => h.MobileHikeId != null).Select(x => x.MobileHikeId).ToList();
                    var ImgDbs = await UOW.ImgRepository.List();
                    var ImgDbMobileIds = ImgDbs.Where(h => h.MobileImgId != null).Select(x => x.MobileImgId).ToList();
                    var ObservationDbs = await UOW.ObservationRepository.List();
                    var ObservationDbMobileIds = ObservationDbs.Where(h => h.MobileObservationId != null).Select(x => x.MobileObservationId).ToList();
                    #endregion

                    #region Create, Update Data
                    foreach (Hike hike in Hikes)
                    {
                        hike.MobileHikeId = hike.Id;
                        hike.TypeId = hike.TypeId == 0 ? null : hike.TypeId;
                        List<Img> imgs = hike.Imgs != null ? hike.Imgs : new List<Img>();
                        imgs.ForEach(i => i.MobileImgId = i.Id);

                        List<Observation> observations = hike.Observations != null ? hike.Observations : new List<Observation>();
                        observations.ForEach(o => o.MobileObservationId = o.Id);

                        if (!HikeDbMobileIds.Contains(hike.MobileHikeId))
                        {
                            hike.Observations?.ForEach(o => o.HikeId = null);
                            isGetDataSuccess = await UOW.HikeRepository.Create(hike);
                        }
                        else
                        {
                            var oldHikeId = HikeDbs.Where(h => h.MobileHikeId == hike.MobileHikeId).Select(h => h.Id).FirstOrDefault();
                            Hike newUpdateHike = hike;
                            newUpdateHike.Id = oldHikeId;
                            isGetDataSuccess = await UOW.HikeRepository.Update(newUpdateHike);
                        }

                        imgs.ForEach(i => i.HikeId = hike.Id);
                        observations.ForEach(i => i.HikeId = hike.Id);

                        if (isGetDataSuccess)
                            isGetDataSuccess = await UOW.ImgRepository.BulkMerge(imgs);
                        else
                            return false;

                        if (isGetDataSuccess)
                            isGetDataSuccess = await UOW.ObservationRepository.BulkMerge(observations);
                        else
                            return false;
                    }
                    #endregion

                    #region Delete Hike does not exist in mobile
                    foreach (long HikeDbMobileId in HikeDbMobileIds)
                    {
                        var MobileHikeIds = Hikes.Select(h => h.MobileHikeId).ToList();
                        if (!MobileHikeIds.Contains(HikeDbMobileId))
                        {
                            var deleteHike = HikeDbs.Where(h => h.MobileHikeId == HikeDbMobileId).FirstOrDefault();
                            await UOW.HikeRepository.Delete(deleteHike);
                        }
                    }
                    #endregion
                }
                else
                    isGetDataSuccess = await UOW.HikeRepository.DeleteAll();

                return isGetDataSuccess;
            }
            catch (Exception ex)
            {

            }
            return false;
        }
    }
}
