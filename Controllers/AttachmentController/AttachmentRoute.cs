using System.ComponentModel;
using System.Reflection;

namespace CodeBE_LEM.Controllers.AttachmentController
{
    [DisplayName("Attachment")]
    public class AttachmentRoute
    {
        public const string Module = "/lem/attachment";
        public const string UploadFile = Module + "/upload-file";
        public const string DowloadFile = Module + "/download-file";
        public const string DeleteFile = Module + "/delete-file";
    }
}
