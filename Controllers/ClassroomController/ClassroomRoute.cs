using System.ComponentModel;

namespace CodeBE_LEM.Controllers.ClassroomController
{
    [DisplayName("Classroom")]
    public class ClassroomRoute
    {
        public const string Module = "/tel/classroom";
        public const string Create = Module + "/create";
        public const string Get = Module + "/get";
        public const string List = Module + "/list";
        public const string Update = Module + "/update";
        public const string Delete = Module + "/delete";

        public const string CreateClassEvent = Module + "/create-class-event";
        public const string GetClassEvent = Module + "/get-class-event";
        public const string ListClassEvent = Module + "/list-class-event";
        public const string UpdateClassEvent = Module + "/update-class-event";
        public const string DeleteClassEvent = Module + "/delete-class-event";

    }
}
