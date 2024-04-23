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
        public const string RoleModule = "lem/role";
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
            { 
                ActionEnum.LIST_PERMISSON.Name, new List<string>()
                {
                    ListPermission, ListPermissionByRole
                }
            },
            { 
                ActionEnum.LIST_ROLE.Name, new List<string>()
                {
                    ListRole, GetRole
                }
            },
            { 
                ActionEnum.CREATE_ROLE.Name, new List<string>()
                {
                    CreateRole
                }
            },
            { 
                ActionEnum.UPDATE_ROLE.Name, new List<string>()
                {
                    UpdateRole
                }
            },
            { 
                ActionEnum.DELETE_ROLE.Name, new List<string>()
                {
                    DeleteRole
                }
            }
        };
    }
}
