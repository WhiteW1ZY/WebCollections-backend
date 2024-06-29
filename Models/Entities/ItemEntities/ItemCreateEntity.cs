using backend.Models.Fields;

namespace backend.Models.Entities.ItemEntities
{
    public class ItemCreateEntity
    {
        public int CollectionId { get; set; }
        public List<string> Tags {  get; set; }
        public IFormFile Image { get; set; }
        public string Name { get; set; }
    }
}
