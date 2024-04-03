using CodeBE_LEM.Enums;
using Microsoft.Win32;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CodeBE_LEM.Controllers.PermissionController
{
    [DisplayName("Permission")]
    public class PermissionRoute
    {
        public const string Module = "lem/permission";
        public const string RoleModule = "/role";
        public const string Init = Module + "/init";
        public const string ListAllPath = Module + "/list-all-path";
        public const string ListPath = Module + "/list-path";
        public const string ListPermission = Module + "/list-permission";
        public const string ListPermissionByRole = Module + "/list-permission-by-role";

        public const string ListRole = RoleModule + "/list-role";
        public const string GetRole = RoleModule + "/get-role";
        public const string CreateRole = RoleModule + "/create-role";
        public const string UpdateRole = RoleModule + "/update-role";
        public const string DeleteRole = RoleModule + "/delete-role";

        public static Dictionary<string, List<string>> DictionaryPath = new Dictionary<string, List<string>>
        {
            { ActionEnum.CREATE.Name, new List<string>()
                {
                    CreateRole
                }
            },
            { ActionEnum.UPDATE.Name, new List<string>()
                {
                    UpdateRole
                }
            },
            { ActionEnum.DELETE.Name, new List<string>()
                {
                    DeleteRole
                }
            },
            { ActionEnum.READ.Name, new List<string>()
                {
                    ListRole, GetRole, ListPermission, ListPermissionByRole
                }
            }
        };
    }
}
