using CodeBE_LEM.Entities;

namespace CodeBE_LEM.Controllers.ClassroomController
{
    public class Classroom_StudentAnswerDTO
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public long QuestionId { get; set; }

        public long AppUserId { get; set; }
        public long? AppUserFeedbackId { get; set; }

        public long? Grade { get; set; }
        public long? ClassEventId { get; set; }

        public DateTime? GradeAt { get; set; }

        public string? Feedback { get; set; }

        public DateTime? SubmitAt { get; set; }
        public Classroom_QuestionDTO? Question { get; set; } = null!;
        public Classroom_AppUserDTO? AppUser { get; set; } = null!;
        public Classroom_AppUserDTO? AppUserFeedback { get; set; } = null!;

        public Classroom_StudentAnswerDTO() { }
        public Classroom_StudentAnswerDTO(StudentAnswer StudentAnswer)
        {
            this.Id = StudentAnswer.Id;
            this.Name = StudentAnswer.Name;
            this.QuestionId = StudentAnswer.QuestionId;
            this.AppUserId = StudentAnswer.AppUserId;
            this.AppUserFeedbackId = StudentAnswer.AppUserFeedbackId;
            this.Grade = StudentAnswer.Grade;
            this.GradeAt = StudentAnswer.GradeAt;
            this.Feedback = StudentAnswer.Feedback;
            this.SubmitAt = StudentAnswer.SubmitAt;
            this.AppUserFeedback = StudentAnswer.AppUserFeedback == null ? null : new Classroom_AppUserDTO(StudentAnswer.AppUserFeedback);
            this.AppUser = StudentAnswer.AppUser == null ? null : new Classroom_AppUserDTO(StudentAnswer.AppUser);
            this.Question = StudentAnswer.Question == null ? null : new Classroom_QuestionDTO(StudentAnswer.Question);
        }
    }
}
