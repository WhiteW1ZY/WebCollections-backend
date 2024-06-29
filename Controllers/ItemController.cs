using backend.Filters;
using backend.Models.Entities.ItemEntities;
using backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : Controller
    {
        private readonly ItemService _service;

        public ItemController(ItemService service)
        {
            _service = service;
        }

        [HttpGet("/getItems")]
        public async Task<IResult> GetItems()
        {
            return await _service.GetItems();
        }

        [HttpPost("/createItems")]
        [Authorize]
        public async Task<IResult> CreateItem(ItemCreateEntity itemCreateEntity)
        {
            return await _service.CreateItem(itemCreateEntity);
        }

        [HttpPut("/updateItem")]
        [ServiceFilter(typeof(ItemAccessControllerFilter))]
        public async Task<IResult> UpdateItem(ItemUpdateEntity itemEntity, int id)
        {
            return await _service.UpdateItem(id, itemEntity);
        }

        [HttpDelete("/deleteItem")]
        [ServiceFilter(typeof(ItemAccessControllerFilter))]
        public async Task<IResult> DeleteItem(int id)
        {
            return await _service.DeleteItem(id);
        }
    }
}
