using CodeBE_LEM.Entities;

namespace CodeBE_LEM.Controllers.ClassroomController
{
    public class Classroom_QuestionDTO
    {
        public long Id { get; set; }

        public long ClassEventId { get; set; }

        public string Name { get; set; } = null!;

        public string QuestionAnswer { get; set; } = null!;

        public string? StudentAnswer { get; set; }

        public string? Description { get; set; }

        public Classroom_QuestionDTO() { }
        public Classroom_QuestionDTO(Question Question)
        {
            Id = Question.Id;
            ClassEventId = Question.ClassEventId;
            Name = Question.Name;
            QuestionAnswer = Question.QuestionAnswer;
            StudentAnswer = Question.StudentAnswer;
            Description = Question.Description;
        }
    }
}
