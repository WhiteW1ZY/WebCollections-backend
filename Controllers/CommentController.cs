using backend.Filters;
using backend.Models.Entities.CommentEntities;
using backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _service;

        public CommentController(CommentService service)
        {
            _service = service;
        }

        [HttpGet("/getComments")]
        public async Task<IResult> GetComments()
        {
            return await _service.GetComments();
        }

        [Authorize]
        [HttpPost("/createComment")]
        public async Task<IResult> CreateComment([FromBody] CreateCommentEntity commentEntity)
        {
            return await _service.CreateComment(commentEntity);
        }

        [HttpPut("/updateComment")]
        [ServiceFilter(typeof(CommentAccessControllerFilter))]
        public async Task<IResult> UpdateComment([FromQuery] int id, [FromBody] CommentUpdateEntity comment)
        {
            return await _service.UpdateComment(id, comment);
        }

        [HttpDelete("/deleteComment")]
        [ServiceFilter(typeof(CommentAccessControllerFilter))]
        public async Task<IResult> DeleteComment([FromQuery] int id)
        {
            return await _service.DeleteComment(id);
        }
    }
}
