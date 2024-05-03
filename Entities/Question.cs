using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Entities;

public partial class Question
{
    public long Id { get; set; }

    public long ClassEventId { get; set; }

    public string Name { get; set; } = null!;

    public string CorrectAnswer { get; set; } = null!;

    public string? StudentAnswer { get; set; }

    public string? Description { get; set; }

    public string? Instruction { get; set; }

    public List<Answer>? Answers { get; set; } = null!;

    public List<StudentAnswer>? StudentAnswers { get; set; } = null!;

    public List<Attachment>? Attachments { get; set; } = null!;

    public ClassEvent ClassEvent { get; set; } = null!;
}
