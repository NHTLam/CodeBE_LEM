using CodeBE_LEM.Common;
using CodeBE_LEM.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeBE_LEM.Controllers.ClassroomController
{
    public partial class ClassroomController
    {
        [Route(ClassroomRoute.ListClassEvent), HttpPost, Authorize]
        public async Task<ActionResult<List<Classroom_ClassEventDTO>>> ListClassEvent([FromBody] FilterDTO FilterDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            List<ClassEvent> ClassEvents = await ClassEventService.List(FilterDTO);
            List<Classroom_ClassEventDTO> Classroom_ClassEventDTOs = ClassEvents
                .Select(c => new Classroom_ClassEventDTO(c)).ToList();

            return Classroom_ClassEventDTOs;
        }

        [Route(ClassroomRoute.GetClassEvent), HttpPost, Authorize]
        public async Task<ActionResult<Classroom_ClassEventDTO>?> GetClassEvent([FromBody] Classroom_ClassEventDTO Classroom_ClassEventDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ClassEvent ClassEvent = await ClassEventService.Get(Classroom_ClassEventDTO.Id);

            return new Classroom_ClassEventDTO(ClassEvent);
        }

        [Route(ClassroomRoute.CreateClassEvent), HttpPost, Authorize]
        public async Task<ActionResult<Classroom_ClassEventDTO>?> CreateClassEvent([FromBody] Classroom_ClassEventDTO Classroom_ClassEventDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ClassEvent ClassEvent = ConvertClassEventDTOToEntity(Classroom_ClassEventDTO);

            ClassEvent = await ClassEventService.Create(ClassEvent);

            return new Classroom_ClassEventDTO(ClassEvent);
        }

        [Route(ClassroomRoute.UpdateClassEvent), HttpPost, Authorize]
        public async Task<ActionResult<Classroom_ClassEventDTO>?> UpdateClassEvent([FromBody] Classroom_ClassEventDTO Classroom_ClassEventDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ClassEvent ClassEvent = ConvertClassEventDTOToEntity(Classroom_ClassEventDTO);

            ClassEvent = await ClassEventService.Update(ClassEvent);

            return new Classroom_ClassEventDTO(ClassEvent);
        }

        [Route(ClassroomRoute.DeleteClassEvent), HttpPost, Authorize]
        public async Task<ActionResult<Classroom_ClassEventDTO>?> DeleteClassEvent([FromBody] Classroom_ClassEventDTO Classroom_ClassEventDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            ClassEvent ClassEvent = ConvertClassEventDTOToEntity(Classroom_ClassEventDTO);

            ClassEvent = await ClassEventService.Delete(ClassEvent);

            return new Classroom_ClassEventDTO(ClassEvent);
        }

        private ClassEvent ConvertClassEventDTOToEntity(Classroom_ClassEventDTO Classroom_ClassEventDTO)
        {
            ClassEvent ClassEvent = new ClassEvent();
            ClassEvent.Id = Classroom_ClassEventDTO.Id;
            ClassEvent.ClassroomId = Classroom_ClassEventDTO.ClassroomId;
            ClassEvent.AppUserId = Classroom_ClassEventDTO.AppUserId.Value == null ? 0 : Classroom_ClassEventDTO.AppUserId.Value;
            ClassEvent.Code = Classroom_ClassEventDTO.Code;
            ClassEvent.Name = Classroom_ClassEventDTO.Name;
            ClassEvent.IsClassWork = Classroom_ClassEventDTO.IsClassWork;
            ClassEvent.StartAt = Classroom_ClassEventDTO.StartAt;
            ClassEvent.Description = Classroom_ClassEventDTO.Description;
            ClassEvent.Pinned = Classroom_ClassEventDTO.Pinned;
            ClassEvent.CreatedAt = Classroom_ClassEventDTO.CreatedAt;
            ClassEvent.EndAt = Classroom_ClassEventDTO.EndAt;
            ClassEvent.DeletedAt = Classroom_ClassEventDTO.DeletedAt;
            ClassEvent.UpdatedAt = Classroom_ClassEventDTO.UpdatedAt;

            ClassEvent.Comments = Classroom_ClassEventDTO.Comments?
                .Select(x => new Comment
                {
                    Id = x.Id,
                    ClassEventId = x.ClassEventId,
                    Description = x.Description,
                }).ToList();

            ClassEvent.Questions = Classroom_ClassEventDTO.Questions?
                .Select(x => new Question
                {
                    Id = x.Id,
                    ClassEventId = x.ClassEventId,
                    Description = x.Description,
                    Name = x.Name,
                    CorrectAnswer = x.CorrectAnswer,
                    StudentAnswer = x.StudentAnswer,
                }).ToList();

            return ClassEvent;
        }
    }
}
