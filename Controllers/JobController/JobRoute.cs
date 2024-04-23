using CodeBE_LEM.Enums;
using System.ComponentModel;
using System.Reflection;

namespace CodeBE_LEM.Controllers.JobController
{
    [DisplayName("Job")]
    public class JobRoute
    {
        public const string Module = "/lem/job";
        public const string Create = Module +"/create";
        public const string Get = Module + "/get";
        public const string List = Module + "/list";
        public const string ListOwn = Module + "/list-own";
        public const string Update = Module + "/update";
        public const string Delete = Module + "/delete";

        public static Dictionary<string, List<string>> DictionaryPath = new Dictionary<string, List<string>>
        {
            {
                ActionEnum.CREATE_JOB.Name, new List<string>()
                {
                    Create
                }
            },
            {
                ActionEnum.UPDATE_JOB.Name, new List<string>()
                {
                    Update
                }
            },
            {
                ActionEnum.DELETE_JOB.Name, new List<string>()
                {
                    Delete
                }
            }
        };
    }
}
