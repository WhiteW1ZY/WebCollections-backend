using backend.Models.Fields;

namespace backend.Models.Entities.ItemEntities
{
    public class ItemUpdateEntity
    {
        public IFormFile Image { get; set; }
        public string Name { get; set; }
        public List<string> Tags { get; set; }
    }
}
