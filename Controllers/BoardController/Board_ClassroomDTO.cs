using CodeBE_LEM.Entities;

namespace CodeBE_LEM.Controllers.BoardController
{
    public class Board_ClassroomDTO
    {
        public long Id { get; set; }

        public string? Code { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public DateTime? DeletedAt { get; set; }

        public string? HomeImg { get; set; }

        public Board_ClassroomDTO() { }
        public Board_ClassroomDTO(Classroom Classroom)
        {
            Id = Classroom.Id;
            Code = Classroom.Code;
            Name = Classroom.Name;
            Description = Classroom.Description;
            CreatedAt = Classroom.CreatedAt;
            UpdatedAt = Classroom.UpdatedAt;
            DeletedAt = Classroom.DeletedAt;
            HomeImg = Classroom.HomeImg;
        }
    }
}
