using CodeBE_LEM.Entities;
using CodeBE_LEM.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeBE_LEM.Repositories
{
    public interface IClassroomRepository
    {
        Task<List<Classroom>> List();
        Task<Classroom> Get(long Id);
        Task<bool> Create(Classroom Classroom);
        Task<bool> Update(Classroom Classroom);
        Task<bool> Delete(Classroom Classroom);
        Task<bool> UpdateCode(Classroom Classroom);
    }

    public class ClassroomRepository : IClassroomRepository
    {
        private DataContext DataContext;
        public ClassroomRepository(DataContext DataContext)
        {
            this.DataContext = DataContext;
        }

        public async Task<bool> Create(Classroom Classroom)
        {
            ClassroomDAO ClassroomDAO = new ClassroomDAO();
            ClassroomDAO.Code = Classroom.Code;
            ClassroomDAO.Name = Classroom.Name;
            ClassroomDAO.Description = Classroom.Description;
            ClassroomDAO.CreatedAt = Classroom.CreatedAt;
            ClassroomDAO.UpdatedAt = Classroom.UpdatedAt;
            ClassroomDAO.DeletedAt = Classroom.DeletedAt;
            DataContext.Classrooms.Add(ClassroomDAO);
            await DataContext.SaveChangesAsync();
            Classroom.Id = ClassroomDAO.Id;
            await SaveReference(Classroom);
            return true;
        }

        public async Task<bool> Delete(Classroom Classroom)
        {
            ClassroomDAO? ClassroomDAO = DataContext.Classrooms
                .Where(x => x.Id == Classroom.Id)
                .FirstOrDefault();
            if (ClassroomDAO == null)
                return false;
            ClassroomDAO.DeletedAt = DateTime.Now;
            await DataContext.SaveChangesAsync();
            await SaveReference(Classroom);
            return true;
        }

        public async Task<Classroom> Get(long Id)
        {
            Classroom? Classroom = await DataContext.Classrooms.AsNoTracking()
                .Where(x => x.DeletedAt == null)
                .Where(x => x.Id == Id)
                .Select(x => new Classroom
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    Description = x.Description,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    DeletedAt = x.DeletedAt,
                }).FirstOrDefaultAsync();

            if (Classroom == null)
                return null;

            Classroom.ClassEvents = await DataContext.ClassEvents.AsNoTracking()
                .Where(x => x.ClassroomId == Classroom.Id)
                .Select(x => new ClassEvent
                {
                    Id = x.Id,
                    ClassroomId = x.ClassroomId,
                    Description = x.Description,

                }).ToListAsync();

            Classroom.AppUserClassroomMappings = await DataContext.AppUserClassroomMappings.AsNoTracking()
                .Where(x => x.ClassroomId == Classroom.Id)
                .Select(x => new AppUserClassroomMapping
                {
                    Id = x.Id,
                    ClassroomId = x.ClassroomId,
                    AppUserId = x.AppUserId,

                }).ToListAsync();

            return Classroom;
        }

        public async Task<List<Classroom>> List()
        {
            List<Classroom> Classrooms = await DataContext.Classrooms.AsNoTracking()
            .Where(x => x.DeletedAt == null)
            .Select(x => new Classroom
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                Description = x.Description,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                DeletedAt = x.DeletedAt,
            }).ToListAsync();

            List<ClassEvent> ClassEvents = await DataContext.ClassEvents.AsNoTracking()
                .Select(x => new ClassEvent
                {
                    Id = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    ClassroomId = x.ClassroomId,
                    Description = x.Description,
                    Instruction = x.Instruction,
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
                }).ToListAsync();

            List<AppUserClassroomMapping> AppUserClassroomMappings = await DataContext.AppUserClassroomMappings.AsNoTracking()
                .Select(x => new AppUserClassroomMapping
                {
                    Id = x.Id,
                    ClassroomId = x.ClassroomId,
                    AppUserId = x.AppUserId,
                }).ToListAsync();

            foreach (Classroom Classroom in Classrooms)
            {
                Classroom.ClassEvents = ClassEvents
                    .Where(x => x.ClassroomId == Classroom.Id)
                    .ToList();
            }

            return Classrooms;
        }

        public async Task<bool> Update(Classroom Classroom)
        {
            ClassroomDAO? ClassroomDAO = DataContext.Classrooms
                .Where(x => x.Id == Classroom.Id)
                .FirstOrDefault();
            if (ClassroomDAO == null)
                return false;
            ClassroomDAO.Code = Classroom.Code;
            ClassroomDAO.Name = Classroom.Name;
            ClassroomDAO.Description = Classroom.Description;
            ClassroomDAO.CreatedAt = Classroom.CreatedAt;
            ClassroomDAO.UpdatedAt = Classroom.UpdatedAt;
            ClassroomDAO.DeletedAt = Classroom.DeletedAt;
            await DataContext.SaveChangesAsync();
            await SaveReference(Classroom);
            return true;
        }

        public async Task<bool> UpdateCode(Classroom Classroom)
        {
            ClassroomDAO? ClassroomDAO = DataContext.Classrooms
                .Where(x => x.Id == Classroom.Id)
                .FirstOrDefault();
            if (ClassroomDAO == null)
                return false;
            ClassroomDAO.Id = Classroom.Id;
            ClassroomDAO.Code = Classroom.Code;
            await DataContext.SaveChangesAsync();
            return true;
        }

        private async Task SaveReference(Classroom Classroom)
        {
            if (Classroom.ClassEvents == null || Classroom.ClassEvents.Count == 0)
            {
                await DataContext.ClassEvents
                    .Where(x => x.ClassroomId == Classroom.Id)
                    .DeleteFromQueryAsync();
            }
            else
            {
                var ClassEventIds = Classroom.ClassEvents.Select(x => x.Id).Distinct().ToList();
                await DataContext.ClassEvents
                    .Where(x => x.ClassroomId == Classroom.Id)
                    .Where(x => !ClassEventIds.Contains(x.Id))
                    .DeleteFromQueryAsync();

                List<ClassEventDAO> ClassEventDAOs = new List<ClassEventDAO>();
                foreach (ClassEvent ClassEvent in Classroom.ClassEvents)
                {
                    ClassEventDAO ClassEventDAO = new ClassEventDAO();
                    ClassEventDAO.Id = ClassEvent.Id;
                    ClassEventDAO.ClassroomId = ClassEvent.ClassroomId;
                    ClassEventDAO.Code = ClassEvent.Code;
                    ClassEventDAO.Name = ClassEvent.Name;
                    ClassEventDAO.IsClassWork = ClassEvent.IsClassWork;
                    ClassEventDAO.Description = ClassEvent.Description;
                    ClassEventDAO.Pinned = ClassEvent.Pinned;
                    ClassEventDAO.Instruction = ClassEvent.Instruction;
                    ClassEventDAO.CreatedAt = ClassEvent.CreatedAt;
                    ClassEventDAO.EndAt = ClassEvent.EndAt;
                    ClassEventDAO.UpdatedAt = ClassEvent.UpdatedAt;
                    ClassEventDAO.DeletedAt = ClassEvent.DeletedAt;
                    ClassEventDAOs.Add(ClassEventDAO);
                }
                await DataContext.BulkMergeAsync(ClassEventDAOs);
            }

            if (Classroom.AppUserClassroomMappings == null || Classroom.AppUserClassroomMappings.Count == 0)
            {
                await DataContext.AppUserClassroomMappings
                    .Where(x => x.ClassroomId == Classroom.Id)
                    .DeleteFromQueryAsync();
            }
            else
            {
                var AppUserClassroomMappingIds = Classroom.AppUserClassroomMappings.Select(x => x.Id).Distinct().ToList();
                await DataContext.AppUserClassroomMappings
                    .Where(x => x.ClassroomId == Classroom.Id)
                    .Where(x => !AppUserClassroomMappingIds.Contains(x.Id))
                    .DeleteFromQueryAsync();

                List<AppUserClassroomMappingDAO> AppUserClassroomMappingDAOs = new List<AppUserClassroomMappingDAO>();
                foreach (AppUserClassroomMapping AppUserClassroomMapping in Classroom.AppUserClassroomMappings)
                {
                    AppUserClassroomMappingDAO AppUserClassroomMappingDAO = new AppUserClassroomMappingDAO();
                    AppUserClassroomMappingDAO.Id = AppUserClassroomMapping.Id;
                    AppUserClassroomMappingDAO.ClassroomId = AppUserClassroomMapping.ClassroomId;
                    AppUserClassroomMappingDAO.AppUserId = AppUserClassroomMapping.AppUserId;
                    AppUserClassroomMappingDAOs.Add(AppUserClassroomMappingDAO);
                }
                await DataContext.BulkMergeAsync(AppUserClassroomMappingDAOs);
            }
        }
    }
}
