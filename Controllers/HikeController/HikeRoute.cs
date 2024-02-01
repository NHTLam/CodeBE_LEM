using System.ComponentModel;
using System.Reflection;

namespace HikeBE.Controllers.HikeController
{
    [DisplayName("Hike")]
    public class HikeRoute
    {
        public const string Module = "/hike";
        public const string Create = Module +"/create";
        public const string Get = Module + "/get";
        public const string List = Module + "/list";
        public const string Update = Module + "/update";
        public const string Delete = Module + "/delete";
        public const string Detail = Module + "/detail";
        public const string UploadImg = Module + "/upload-img";
        public const string GetImg = Module + "/get-img";
        public const string BulkDeleteImg = Module + "/bulk-delete-img";
        public const string GetDataFromMobile = Module + "/get-data-from-mobile";
    }
}
