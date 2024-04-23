using CodeBE_LEM.Common;
using CodeBE_LEM.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeBE_LEM.Controllers.ClassroomController
{
    public partial class ClassroomController
    {
        [Route(ClassroomRoute.DetailStudentAnswer), HttpPost]
        public async Task<ActionResult<List<Classroom_StudentAnswerDTO>>?> DetailStudentAnswer([FromBody] Classroom_StudentAnswerDTO Classroom_StudentAnswerDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            StudentAnswer StudentAnswer = ConvertStudentAnswerDTOToEntity(Classroom_StudentAnswerDTO);

            List<StudentAnswer> StudentAnswers = await ClassEventService.DetailStudentAnswer(StudentAnswer);
            List<Classroom_StudentAnswerDTO> Classroom_StudentAnswerDTOs = StudentAnswers
                .Select(x => new Classroom_StudentAnswerDTO(x)).ToList();

            return Classroom_StudentAnswerDTOs;
        }

        [Route(ClassroomRoute.ListStudentAnswer), HttpPost]
        public async Task<ActionResult<List<Classroom_AppUserDTO>>?> ListStudentAnswer([FromBody] Classroom_StudentAnswerDTO Classroom_StudentAnswerDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            StudentAnswer StudentAnswer = ConvertStudentAnswerDTOToEntity(Classroom_StudentAnswerDTO);

            List<AppUser> AppUsers = await ClassEventService.ListStudentAnswer(StudentAnswer);
            List<Classroom_AppUserDTO> Classroom_AppUserDTOs = AppUsers
                .Select(x => new Classroom_AppUserDTO(x)).ToList();

            return Classroom_AppUserDTOs;
        }

        [Route(ClassroomRoute.CreateStudentAnswer), HttpPost]
        public async Task<ActionResult<Classroom_StudentAnswerDTO>?> CreateStudentAnswer([FromBody] Classroom_StudentAnswerDTO Classroom_StudentAnswerDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            StudentAnswer StudentAnswer = ConvertStudentAnswerDTOToEntity(Classroom_StudentAnswerDTO);

            StudentAnswer = await ClassEventService.CreateStudentAnswer(StudentAnswer);

            return new Classroom_StudentAnswerDTO(StudentAnswer);
        }

        [Route(ClassroomRoute.UpdateStudentAnswer), HttpPost]
        public async Task<ActionResult<Classroom_StudentAnswerDTO>?> UpdateStudentAnswer([FromBody] Classroom_StudentAnswerDTO Classroom_StudentAnswerDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            StudentAnswer StudentAnswer = ConvertStudentAnswerDTOToEntity(Classroom_StudentAnswerDTO);

            StudentAnswer = await ClassEventService.UpdateStudentAnswer(StudentAnswer);

            return new Classroom_StudentAnswerDTO(StudentAnswer);
        }

        private StudentAnswer ConvertStudentAnswerDTOToEntity(Classroom_StudentAnswerDTO Classroom_StudentAnswerDTO)
        {
            StudentAnswer StudentAnswer = new StudentAnswer();
            StudentAnswer.Id = Classroom_StudentAnswerDTO.Id;
            StudentAnswer.QuestionId = Classroom_StudentAnswerDTO.QuestionId;
            StudentAnswer.AppUserId = Classroom_StudentAnswerDTO.AppUserId;
            StudentAnswer.ClassEventId = Classroom_StudentAnswerDTO.ClassEventId;
            StudentAnswer.Name = Classroom_StudentAnswerDTO.Name;
            StudentAnswer.AppUserFeedbackId = Classroom_StudentAnswerDTO.AppUserFeedbackId;
            StudentAnswer.Grade = Classroom_StudentAnswerDTO.Grade;
            StudentAnswer.Feedback = Classroom_StudentAnswerDTO.Feedback;

            return StudentAnswer;
        }
    }
}
