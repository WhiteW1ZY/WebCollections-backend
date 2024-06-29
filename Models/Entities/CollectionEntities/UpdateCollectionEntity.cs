namespace backend.Models.Entities.CollectionEntities
{
    public class UpdateCollectionEntity
    {
        public int id { get; set; }
        public string Name { get; set; }
        public IFormFile Image { get; set; }
    }
}
