using CodeBE_LEM.Entities;

namespace CodeBE_LEM.Controllers.ClassroomController
{
    public class Classroom_AnswerDTO
    {
        public long Id { get; set; }

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public long QuestionId { get; set; }

        public Classroom_AnswerDTO() { }
        public Classroom_AnswerDTO(Answer Answer)
        {
            Id = Answer.Id;
            Code = Answer.Code;
            Name = Answer.Name;
            QuestionId = Answer.QuestionId;
        }
    }
}
