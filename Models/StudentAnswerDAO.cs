using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Models;

public partial class StudentAnswerDAO
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long QuestionId { get; set; }

    public long AppUserId { get; set; }

    public long? AppUserFeedbackId { get; set; }

    public long? Grade { get; set; }

    public DateTime? GradeAt { get; set; }

    public string? Feedback { get; set; }

    public DateTime? SubmitAt { get; set; }

    public virtual AppUserDAO AppUser { get; set; } = null!;

    public virtual AppUserDAO? AppUserFeedback { get; set; } = null!;

    public virtual QuestionDAO Question { get; set; } = null!;
}
