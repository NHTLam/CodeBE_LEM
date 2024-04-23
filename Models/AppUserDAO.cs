using CodeBE_LEM.Models;
using System;
using System.Collections.Generic;

namespace CodeBE_LEM.Models;

public partial class AppUserDAO
{
    public long Id { get; set; }

    public string? FullName { get; set; }

    public string UserName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Gender { get; set; }

    public string Password { get; set; } = null!;

    public long StatusId { get; set; }

    public virtual ICollection<AppUserBoardMappingDAO> AppUserBoardMappings { get; set; } = new List<AppUserBoardMappingDAO>();

    public virtual ICollection<AppUserClassroomMappingDAO> AppUserClassroomMappings { get; set; } = new List<AppUserClassroomMappingDAO>();

    public virtual ICollection<AppUserJobMappingDAO> AppUserJobMappings { get; set; } = new List<AppUserJobMappingDAO>();

    public virtual ICollection<AttachmentDAO> Attachments { get; set; } = new List<AttachmentDAO>();

    public virtual ICollection<ClassEventDAO> ClassEvents { get; set; } = new List<ClassEventDAO>();

    public virtual ICollection<JobDAO> Jobs { get; set; } = new List<JobDAO>();

    public virtual ICollection<CommentDAO> Comments { get; set; } = new List<CommentDAO>();

    public virtual ICollection<StudentAnswerDAO> StudentAnswers { get; set; } = new List<StudentAnswerDAO>();

    public virtual ICollection<StudentAnswerDAO> StudentAnswerFeedbacks { get; set; } = new List<StudentAnswerDAO>();
}
