
using backend.Filters;
using backend.Models.Entities.CollectionEntities;
using backend.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class CollectionController : Controller
    {
        private readonly CollectionService _service;

        public CollectionController(CollectionService service)
        {
            _service = service;
        }

        [HttpGet("/getCollections")]
        public async Task<IResult> GetCollections()
        {
            return await _service.GetCollections();
        }

        [Authorize]
        [HttpPost("/createCollection")]
        public async Task<IResult> CreateCollection(CreateCollectionEntity collectionEntity)
        {
            return await _service.CreateCollection(collectionEntity);
        }

       
        [HttpPut("/updateCollection")]
        [ServiceFilter(typeof(CollectionAccessControllerFilter))]
        public async Task<IResult> UpdateCollection([FromQuery] int id, UpdateCollectionEntity collectionEntity)
        {
            return await _service.UpdateCollection(collectionEntity);
        }

        [HttpDelete("/deleteCollection")]
        [ServiceFilter(typeof(CollectionAccessControllerFilter))]
        public async Task<IResult> DeleteCollection(int id)
        {
            return await _service.DeleteCollection(id);
        }
    }
}
