using backend.Filters.FieldFilters;
using backend.Models.Entities.FieldsEntities.IntegerFieldsValue;
using backend.Service.Fields;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Fields
{
    [Route("api/[controller]")]
    [ApiController]
    public class IntegerFieldController : ControllerBase
    {
        private readonly IntegerFieldService _service;

        public IntegerFieldController(IntegerFieldService service)
        {
            _service = service;
        }

        [HttpGet("/getIntegerFields")]
        public async Task<IResult> GetFields()
        {
            return await _service.GetIntegerFields();
        }

        [Authorize]
        [HttpPost("/createIntegerField")]
        public async Task<IResult> CreateField([FromBody] CreateIntegerFieldEntity fieldEntity)
        {
            return await _service.CreateIntegerField(fieldEntity);
        }

        [HttpPut("/updateIntegerField")]
        [ServiceFilter(typeof(IntegerFieldAccessControllerFilter))]
        public async Task<IResult> UpdateField([FromBody] UpdateIntegerFieldEntity fieldEntity, [FromQuery] int id)
        {
            return await _service.UpdateIntegerField(id, fieldEntity);
        }

        [HttpDelete("/deleteIntegerField")]
        [ServiceFilter(typeof(IntegerFieldAccessControllerFilter))]
        public async Task<IResult> DeleteField([FromQuery] int id)
        {
            return await _service.DeleteIntegerField(id);
        }
    }
}
