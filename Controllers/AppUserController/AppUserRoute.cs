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
        public const string Update = Module + "/update";
        public const string Delete = Module + "/delete";
        public const string Login = Module + "/login";
        public const string GetUserId = Module + "/get-user-id";

        public static Dictionary<string, List<string>> DictionaryPath = new Dictionary<string, List<string>> 
        {
            { ActionEnum.CREATE.Name, new List<string>() 
                {
                    Register, Login
                } 
            },
            { ActionEnum.UPDATE.Name, new List<string>()
                {
                    Update, Login
                }
            },
            { ActionEnum.DELETE.Name, new List<string>()
                {
                    Delete, Login
                }
            },
            { ActionEnum.READ.Name, new List<string>()
                {
                    List, Get, Login
                }
            }
        };
    }
}
