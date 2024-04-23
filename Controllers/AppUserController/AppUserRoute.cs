using CodeBE_LEM.Enums;
using System.ComponentModel;
using System.Reflection;

namespace CodeBE_LEM.Controllers.AppUserController
{
    [DisplayName("AppUser")]
    public class AppUserRoute
    {
        public const string Module = "/lem/app-user";
        public const string Register = Module + "/register";
        public const string Get = Module + "/get";
        public const string List = Module + "/list";
        public const string ListByClassroom = Module + "/list-by-classroom";
        public const string Update = Module + "/update";
        public const string Delete = Module + "/delete";
        public const string Login = Module + "/login";
        public const string GetUserId = Module + "/get-user-id";

        //Không có dictionary path do các api đều là api cần thiết cho hệ thống hoạt động chứ không phụ vụ cho riêng 1 chức năng
    }
}
