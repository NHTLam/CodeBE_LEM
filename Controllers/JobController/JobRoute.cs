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
        public const string Update = Module + "/update";
        public const string Delete = Module + "/delete";
    }
}
