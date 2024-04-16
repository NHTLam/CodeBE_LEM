using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CodeBE_LEM.Services.JobService;
using CodeBE_LEM.Entities;

namespace CodeBE_LEM.Controllers.JobController
{
    [ApiController]
    public class JobController : ControllerBase
    {
        private IJobService JobService;

        public JobController(
            IJobService JobService
        )
        {
            this.JobService = JobService;
        }

        [Route(JobRoute.List), HttpPost]
        public async Task<ActionResult<List<Job_JobDTO>>> List()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<Job> Jobs = await JobService.List();
            List<Job_JobDTO> Job_JobDTOs = Jobs
                .Select(c => new Job_JobDTO(c)).ToList();

            return Job_JobDTOs;
        }

        [Route(JobRoute.Get), HttpPost]
        public async Task<ActionResult<Job_JobDTO>?> Get([FromBody] Job_JobDTO Job_JobDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Job Job = await JobService.Get(Job_JobDTO.Id);
            if (Job == null)
                return null;
            Job_JobDTO = new Job_JobDTO(Job);
            return Job_JobDTO;
        }

        [Route(JobRoute.Create), HttpPost]
        public async Task<ActionResult<Job_JobDTO>> Create([FromBody] Job_JobDTO Job_JobDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Job Job = ConvertDTOToEntity(Job_JobDTO);

            Job = await JobService.Create(Job);
            Job_JobDTO = new Job_JobDTO(Job);
            if (Job != null)
                return Job_JobDTO;
            else
                return BadRequest(Job_JobDTO);
        }

        [Route(JobRoute.Update), HttpPost]
        public async Task<ActionResult<Job_JobDTO>> Update([FromBody] Job_JobDTO Job_JobDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Job Job = ConvertDTOToEntity(Job_JobDTO);
            Job = await JobService.Update(Job);
            Job_JobDTO = new Job_JobDTO(Job);
            if (Job != null)
                return Job_JobDTO;
            else
                return BadRequest(Job_JobDTO);
        }

        [Route(JobRoute.Delete), HttpPost]
        public async Task<ActionResult<Job_JobDTO>> Delete([FromBody] Job_JobDTO Job_JobDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Job Job = ConvertDTOToEntity(Job_JobDTO);
            Job = await JobService.Delete(Job);
            Job_JobDTO = new Job_JobDTO(Job);
            if (Job != null)
                return Job_JobDTO;
            else
                return BadRequest(Job_JobDTO);
        }

        private Job ConvertDTOToEntity(Job_JobDTO Job_JobDTO)
        {
            Job Job = new Job();
            Job.Id = Job_JobDTO.Id;
            Job.CardId = Job_JobDTO.CardId;
            Job.Name = Job_JobDTO.Name;
            Job.Description = Job_JobDTO.Description;
            Job.Order = Job_JobDTO.Order;
            Job.StartAt = Job_JobDTO.StartAt;
            Job.EndAt = Job_JobDTO.EndAt;
            Job.Color = Job_JobDTO.Color;
            Job.NoTodoDone = Job_JobDTO.NoTodoDone;
            Job.IsAllDay = Job_JobDTO.IsAllDay;
            Job.Todos = Job_JobDTO.Todos?.Select(x => new Todo
            {
                Id = x.Id,
                JobId = x.JobId,
                Description = x.Description,
                CompletePercent = x.CompletePercent,
            }).ToList();

            return Job;
        }
    }
}
