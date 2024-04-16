using CodeBE_LEM.Controllers.AppUserController;
using CodeBE_LEM.Entities;
using CodeBE_LEM.Models;
using CodeBE_LEM.Repositories;
using CodeBE_LEM.Services.AppUserService;
using CodeBE_LEM.Services.PermissionService;
using Microsoft.AspNetCore.Mvc;

namespace CodeBE_LEM.Controllers
{
    public class MeetingController : ControllerBase
    {
        private DataContext DataContext;
        private IPermissionService PermissionService;
        private IUOW UOW;
        private const string ZoomAPIKey = "_c7q5ir4TRi825ePizcUw";
        private const string ZoomAPISecret = "a0noKcesmPxKErfa65WF3iigGeeMaZxw";

        public MeetingController(DataContext DataContext, IPermissionService PermissionService, IUOW uOW)
        {
            this.DataContext = DataContext;
            this.PermissionService = PermissionService;
            UOW = uOW;
        }

        //[Route("/lem/meeting/create-meeting"), HttpPost]
        //public async Task<ActionResult<bool>> CreateMeeting([FromBody] Meeting_MeetingDTO Meeting_MeetingDTO)
        //{
        //    HttpClient client = new HttpClient();
        //    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

        //    //var jwtToken = GenerateJWTToken(ZoomAPIKey, ZoomAPISecret);
            
        //}
    }

    public class Meeting_MeetingDTO
    {
        public string Topic { get; set; }
        public DateTime StartTime { get; set; }
        public float MeetingDuration { get; set; }
        public Meeting_MeetingDTO() { }
        public Meeting_MeetingDTO(string topic, DateTime startTime, float meetingDuration)
        {
            Topic = topic;
            StartTime = startTime;
            MeetingDuration = meetingDuration;
        }
    }
}
