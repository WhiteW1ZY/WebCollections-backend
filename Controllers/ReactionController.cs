using backend.Filters;
using backend.Models.Entities.ReactionEntities;
using backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactionController : ControllerBase
    {
        private readonly ReactionService _service;

        public ReactionController(ReactionService service)
        {
            _service = service;
        }

        [HttpGet("/getReactions")]
        public async Task<IResult> GetReactions()
        {
            return await _service.GetReactions();
        }

        [Authorize]
        [HttpPost("/createReaction")]
        public async Task<IResult> CreateReaction([FromBody] CreateReactionEntity reactionEntity)
        {
            return await _service.CreateReaction(reactionEntity);
        }

        [HttpPut("/updateReaction")]
        [ServiceFilter(typeof(ReactionAccessControllerFilter))]
        public async Task<IResult> UpdateReaction([FromQuery] int id, [FromBody] UpdateReactionEntity reactionEntity)
        {
            return await _service.UpdateReaction(id, reactionEntity);
        }

        [HttpDelete("/deleteReaction")]
        [ServiceFilter(typeof(ReactionAccessControllerFilter))]
        public async Task<IResult> DeleteReaction([FromQuery] int id)
        {
            return await _service.DeleteReaction(id);
        }
    }
}
