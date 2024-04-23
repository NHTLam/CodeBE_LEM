using CodeBE_LEM.Common;
using CodeBE_LEM.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeBE_LEM.Controllers.ClassroomController
{
    public partial class ClassroomController
    {

        [Route(ClassroomRoute.CreateComment), HttpPost, Authorize]
        public async Task<ActionResult<Classroom_CommentDTO>?> CreateComment([FromBody] Classroom_CommentDTO Classroom_CommentDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Comment Comment = ConvertCommentDTOToEntity(Classroom_CommentDTO);

            Comment = await ClassEventService.CreateComment(Comment);

            return Ok();
        }

        [Route(ClassroomRoute.UpdateComment), HttpPost, Authorize]
        public async Task<ActionResult<Classroom_CommentDTO>?> UpdateComment([FromBody] Classroom_CommentDTO Classroom_CommentDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Comment Comment = ConvertCommentDTOToEntity(Classroom_CommentDTO);

            Comment = await ClassEventService.UpdateComment(Comment);

            return Ok();

        }

        [Route(ClassroomRoute.DeleteComment), HttpPost, Authorize]
        public async Task<ActionResult<Classroom_CommentDTO>?> DeleteComment([FromBody] Classroom_CommentDTO Classroom_CommentDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            Comment Comment = ConvertCommentDTOToEntity(Classroom_CommentDTO);

            Comment = await ClassEventService.DeleteComment(Comment);

            return Ok();

        }

        private Comment ConvertCommentDTOToEntity(Classroom_CommentDTO Classroom_CommentDTO)
        {
            Comment Comment = new Comment();
            Comment.Id = Classroom_CommentDTO.Id;
            Comment.ClassEventId = Classroom_CommentDTO.ClassEventId;
            Comment.Description = Classroom_CommentDTO.Description;
            Comment.AppUserId = Classroom_CommentDTO.AppUserId;

            return Comment;
        }
    }
}
