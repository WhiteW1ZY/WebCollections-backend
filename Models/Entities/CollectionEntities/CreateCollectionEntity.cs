using System.Buffers.Text;
using System.ComponentModel.DataAnnotations;

namespace backend.Models.Entities.CollectionEntities
{
    public class CreateCollectionEntity
    {
        public string Name { get; set; }
        public int OwnerId { get; set; }
        public IFormFile Image {  get; set; }
    }
}
