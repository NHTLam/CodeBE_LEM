using CodeBE_LEM.Entities;

namespace CodeBE_LEM.Controllers.ClassroomController
{
    public class Classroom_AppUserDTO
    {
        public long Id { get; set; }

        public string UserName { get; set; } = null!;

        public string? FullName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Gender { get; set; }

        public string? Password { get; set; }

        public long? StatusId { get; set; }

        public Classroom_AppUserDTO() { }
        public Classroom_AppUserDTO(AppUser AppUser)
        {
            this.Id = AppUser.Id;
            this.UserName = AppUser.UserName;
            this.FullName = AppUser.FullName;
            this.Email = AppUser.Email;
            this.Phone = AppUser.Phone;
            this.Gender = AppUser.Gender;
            this.Password = AppUser.Password;
            this.StatusId = AppUser.StatusId;
        }
    }
}
