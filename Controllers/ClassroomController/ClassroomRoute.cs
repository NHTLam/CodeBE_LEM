using CodeBE_LEM.Enums;
using System.ComponentModel;

namespace CodeBE_LEM.Controllers.ClassroomController
{
    [DisplayName("Classroom")]
    public class ClassroomRoute
    {
        public const string Module = "/lem/classroom";
        public const string Create = Module + "/create";
        public const string Get = Module + "/get";
        public const string List = Module + "/list";
        public const string ListOwn = Module + "/list-own";
        public const string Update = Module + "/update";
        public const string Delete = Module + "/delete";
        public const string Join = Module + "/join";
        public const string Kick = Module + "/kick";
        public const string Leave = Module + "/leave";

        public const string CreateClassEvent = Module + "/create-class-event";
        public const string GetClassEvent = Module + "/get-class-event";
        public const string ListClassEvent = Module + "/list-class-event";
        public const string UpdateClassEvent = Module + "/update-class-event";
        public const string DeleteClassEvent = Module + "/delete-class-event";

        public const string CreateComment = Module + "/create-comment";
        public const string UpdateComment = Module + "/update-comment";
        public const string DeleteComment = Module + "/delete-comment";

        public const string CreateQuestion = Module + "/create-question";
        public const string UpdateQuestion = Module + "/update-question";
        public const string DeleteQuestion = Module + "/delete-question";

        public const string ListStudentAnswer = Module + "/list-student-answer";
        public const string DetailStudentAnswer = Module + "/detail-student-answer";
        public const string CreateStudentAnswer = Module + "/create-student-answer";
        public const string UpdateStudentAnswer = Module + "/update-student-answer";

        public const string MarkOrFeedBack = Module + "/mark-or-feedback";
        public const string CreateClassWork = Module + "/create-classwork";

        public static Dictionary<string, List<string>> DictionaryPath = new Dictionary<string, List<string>>
        {
            {
                ActionEnum.KICK_MEMBER.Name, new List<string>()
                {
                    Kick
                }
            },
            {
                ActionEnum.CREATE_QUESTION.Name, new List<string>()
                {
                    CreateQuestion
                }
            },
            {
                ActionEnum.UPDATE_QUESTION.Name, new List<string>()
                {
                    UpdateQuestion
                }
            },
            {
                ActionEnum.DELETE_QUESTION.Name, new List<string>()
                {
                    DeleteQuestion
                }
            },
            {
                ActionEnum.MARK_OR_FEEDBACK.Name, new List<string>()
                {
                    MarkOrFeedBack
                }
            },
            {
                ActionEnum.CREATE_CLASSWORK.Name, new List<string>()
                {
                    CreateClassWork
                }
            }
            ,
            {
                ActionEnum.DELETE_CLASS.Name, new List<string>()
                {
                    Delete
                }
            }
        };
    }
}
