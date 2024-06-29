using backend.Models.Entities.TagEntities;
using backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagController: Controller
    {
        private readonly TagService _service; 

        public TagController(TagService service)
        {
            _service = service;
        }

        [HttpGet("/getTags")]
        public async Task<IResult> GetTags()
        {
            return await _service.GetTags();
        }

        [Authorize]
        [HttpPost("/createTag")]
        public async Task<IResult> CreateTag([FromBody]TagCreateEntity tag)
        {
            return await _service.CreateTag(tag);
        }

        [Authorize(Roles ="admin")]
        [HttpPut("/updateTag")]
        public async Task<IResult> UpdateTag([FromQuery]string tagName, [FromBody] TagUpdateEntity tag)
        {
            return await _service.UpdateTag(tagName, tag);
        }

        [Authorize(Roles ="admin")]
        [HttpDelete("/deleteTag")]
        public async Task<IResult> DeleteTag([FromQuery]string name)
        {
            return await _service.DeleteTag(name);
        }
    }
}
