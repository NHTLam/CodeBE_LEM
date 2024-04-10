using System.ComponentModel;
using System.Reflection;

namespace CodeBE_LEM.Controllers.BoardController
{
    [DisplayName("Board")]
    public class BoardRoute
    {
        public const string Module = "/lem/board";
        public const string Create = Module +"/create";
        public const string Get = Module + "/get";
        public const string GetOwn = Module + "/get-own";
        public const string List = Module + "/list";
        public const string Update = Module + "/update";
        public const string Delete = Module + "/delete";
    }
}
