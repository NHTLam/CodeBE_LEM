using CodeBE_LEM.Enums;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CodeBE_LEM.Controllers.AttachmentController
{
    [DisplayName("Attachment")]
    public class AttachmentRoute
    {
        public const string Module = "/lem/attachment";
        public const string UploadFile = Module + "/upload-file";
        public const string DowloadFile = Module + "/download-file";
        public const string DeleteFile = Module + "/delete-file";

        public static Dictionary<string, List<string>> DictionaryPath = new Dictionary<string, List<string>>
        {
            { 
                ActionEnum.UPLOAD_FILE.Name, new List<string>()
                {
                    UploadFile
                }
            },
            { 
                ActionEnum.DOWLOAD_FILE.Name, new List<string>()
                {
                    DowloadFile
                }
            },
        };
    }
}
