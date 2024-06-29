using backend.Filters.FieldFilters;
using backend.Models.Entities.FieldsEntities.DateFieldsEntities;
using backend.Service.Fields;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Fields
{
    [Route("api/[controller]")]
    [ApiController]
    public class DateFieldController : ControllerBase
    {
        private readonly DateFieldService _service;

        public DateFieldController(DateFieldService service)
        {
            _service = service;
        }

        [HttpGet("/getDateFields")]
        public async Task<IResult> GetFields()
        {
            return await _service.GetFields();
        }

        [Authorize]
        [HttpPost("/createDateField")]
        public async Task<IResult> CreateField([FromBody] CreateDateFieldEntity fieldEntity)
        {
            return await _service.CreateDateField(fieldEntity);
        }

        [HttpPut("/updateDateField")]
        [ServiceFilter(typeof(DateFieldAccessControllerFilter))]
        public async Task<IResult> UpdateField([FromBody] UpdateDateFieldEntity fieldEntity, [FromQuery] int id)
        {
            return await _service.UpdateDateField(id, fieldEntity);
        }

        [HttpDelete("/deleteDateField")]
        [ServiceFilter(typeof(DateFieldAccessControllerFilter))]
        public async Task<IResult> DeleteField([FromQuery] int id)
        {
            return await _service.DeleteDateField(id);
        }
    }
}
