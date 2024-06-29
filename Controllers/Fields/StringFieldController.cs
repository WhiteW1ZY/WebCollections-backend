using backend.Filters.FieldFilters;
using backend.Models.Entities.FieldsEntities.StringFieldsEntities;
using backend.Models.Entities.ItemEntities;
using backend.Service;
using backend.Service.Fields;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Fields
{
    [Route("api/[controller]")]
    [ApiController]
    public class StringFieldController : ControllerBase
    {
        private readonly StringFieldService _service;

        public StringFieldController(StringFieldService service)
        {
            _service = service;
        }

        [HttpGet("/getStringFields")]
        public async Task<IResult> GetFields()
        {
            return await _service.GetStringFields();
        }

        [Authorize]
        [HttpPost("/createStringField")]
        public async Task<IResult> CreateField([FromBody] CreateStringFieldEntity stringFieldEntity)
        {
            return await _service.CreateStringField(stringFieldEntity);
        }

        [HttpPut("/updateStringField")]
        [ServiceFilter(typeof(StringFieldAccessControllerFilter))]
        public async Task<IResult> UpdateField([FromBody] UpdateStringFieldEntity stringFieldEntity, [FromQuery]int id)
        {
            return await _service.UpdateStringField(id, stringFieldEntity);
        }

        [HttpDelete("/deleteStringField")]
        [ServiceFilter(typeof(StringFieldAccessControllerFilter))]
        public async Task<IResult> DeleteField([FromQuery] int id)
        {
            return await _service.DeleteStrignField(id);
        }
    }
}
