using backend.Filters.FieldFilters;
using backend.Models.Entities.FieldsEntities.BooleanFieldsEntities;
using backend.Service.Fields;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Fields
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooleanFieldController : ControllerBase
    {
        private readonly BooleanFieldService _service;

        public BooleanFieldController(BooleanFieldService service)
        {
            _service = service;
        }

        [HttpGet("/getBooleanFields")]
        public async Task<IResult> GetFields()
        {
            return await _service.GetBooleanFields();
        }

        [Authorize]
        [HttpPost("/createBooleanField")]
        public async Task<IResult> CreateField([FromBody] CreateBooleanFieldEntity booleanFieldEntity)
        {
            return await _service.CreateBooleanField(booleanFieldEntity);
        }

        [HttpPut("/updateBooleanField")]
        [ServiceFilter(typeof(BooleanFieldAccessControllerFilter))]
        public async Task<IResult> UpdateField([FromBody] UpdateBooleanFieldEntity booleanFieldEntity, [FromQuery] int id)
        {
            return await _service.UpdateBooleanField(id, booleanFieldEntity);
        }

        [HttpDelete("/deleteBooleanField")]
        [ServiceFilter(typeof(BooleanFieldAccessControllerFilter))]
        public async Task<IResult> DeleteField([FromQuery] int id)
        {
            return await _service.DeleteBooleanField(id);
        }
    }
}
