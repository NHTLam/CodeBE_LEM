using CodeBE_LEM.Entities;

namespace CodeBE_LEM.Entities
{
    public class StudentAnswer
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public long QuestionId { get; set; }

        public long? ClassEventId { get; set; }

        public long AppUserId { get; set; }

        public long? AppUserFeedbackId { get; set; }

        public long? Grade { get; set; }

        public DateTime? GradeAt { get; set; }

        public string? Feedback { get; set; }

        public DateTime? SubmitAt { get; set; }
        public Question? Question { get; set; } = null!;
        public AppUser? AppUser { get; set; } = null!;
        public AppUser? AppUserFeedback { get; set; } = null!;

    }
}
