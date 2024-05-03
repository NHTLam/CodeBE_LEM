using CodeBE_LEM.Entities;
using CodeBE_LEM.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;

namespace CodeBE_LEM.Repositories
{
    public interface IAttachmentRepository
    {
        Task<List<Attachment>> List();
        Task<Attachment> Get(long AttachmentId);
        Task<bool> Create(Attachment Attachment);
        Task<bool> Update(Attachment Attachment);
        Task<bool> Delete(Attachment Attachment);
        Task<List<long>> BulkMerge(List<Attachment> Attachments);
    }

    public class AttachmentRepository : IAttachmentRepository
    {
        private DataContext DataContext;

        public AttachmentRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        public async Task<List<Attachment>> List()
        {
            IQueryable<AttachmentDAO> query = DataContext.Attachments.AsNoTracking();
            List<Attachment> Attachments = await query.AsNoTracking().Select(x => new Attachment
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Path = x.Path,
                Capacity = x.Capacity,
                QuestionId = x.QuestionId,
            }).ToListAsync();

            return Attachments;
        }

        public async Task<Attachment> Get(long Id)
        {
            Attachment? Attachment = await DataContext.Attachments.AsNoTracking()
            .Where(x => x.Id == Id)
            .Select(x => new Attachment()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Path = x.Path,
                Capacity = x.Capacity,
                QuestionId = x.QuestionId,
                OwnerId = x.OwnerId,
                PublicId = x.PublicId,
                Link = x.Link,
            }).FirstOrDefaultAsync();

            return Attachment;
        }

        public async Task<bool> Create(Attachment Attachment)
        {
            AttachmentDAO AttachmentDAO = new AttachmentDAO();
            AttachmentDAO.Name = Attachment.Name;
            AttachmentDAO.Description = Attachment.Description;
            AttachmentDAO.Path = Attachment.Path;
            AttachmentDAO.Capacity = Attachment.Capacity;
            AttachmentDAO.QuestionId = Attachment.QuestionId;
            DataContext.Attachments.Add(AttachmentDAO);
            await DataContext.SaveChangesAsync();
            Attachment.Id = AttachmentDAO.Id;
            await SaveReference(Attachment);
            return true;
        }

        public async Task<bool> Update(Attachment Attachment)
        {
            AttachmentDAO? AttachmentDAO = DataContext.Attachments
                .Where(x => x.Id == Attachment.Id)
                .FirstOrDefault();
            if (AttachmentDAO == null)
                return false;
            AttachmentDAO.Id = Attachment.Id;
            AttachmentDAO.Name = Attachment.Name;
            AttachmentDAO.Description = Attachment.Description;
            AttachmentDAO.Path = Attachment.Path;
            AttachmentDAO.Capacity = Attachment.Capacity;
            AttachmentDAO.QuestionId = Attachment.QuestionId;
            await DataContext.SaveChangesAsync();
            await SaveReference(Attachment);
            return true;
        }

        public async Task<bool> Delete(Attachment Attachment)
        {
            AttachmentDAO? AttachmentDAO = DataContext.Attachments
                .Where(x => x.Id == Attachment.Id)
                .FirstOrDefault();
            if (Attachment == null)
                return false;
            DataContext.Attachments.Remove(AttachmentDAO);
            await DataContext.SaveChangesAsync();
            await SaveReference(Attachment);
            return true;
        }

        private async Task SaveReference(Attachment Attachment)
        {
        }

        public async Task<List<long>> BulkMerge(List<Attachment> Attachments)
        {
            List<AttachmentDAO> AttachmentDAOs = new List<AttachmentDAO>();
            foreach (Attachment Attachment in Attachments)
            {
                AttachmentDAO AttachmentDAO = new AttachmentDAO();
                AttachmentDAO.Id = Attachment.Id;
                AttachmentDAO.Name = Attachment.Name;
                AttachmentDAO.Path = Attachment.Path;
                AttachmentDAO.Description = Attachment.Description;
                AttachmentDAO.Capacity = Attachment.Capacity;
                AttachmentDAO.QuestionId = Attachment.QuestionId;
                AttachmentDAO.OwnerId = Attachment.OwnerId;
                AttachmentDAO.PublicId = Attachment.PublicId;
                AttachmentDAO.Link = Attachment.Link;
                AttachmentDAOs.Add(AttachmentDAO);
                if (AttachmentDAO.Id == 0)
                    DataContext.Attachments.Add(AttachmentDAO);
                else
                    DataContext.Attachments.Update(AttachmentDAO);
            }
            await DataContext.SaveChangesAsync();

            List<long> Ids = AttachmentDAOs.Select(x => x.Id).ToList();
            return Ids;
        }
    }
}
