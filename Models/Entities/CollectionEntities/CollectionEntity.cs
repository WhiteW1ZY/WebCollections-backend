namespace backend.Models.Entities.CollectionEntities
{
    public class CollectionEntity
    {

        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string Image {  get; set; }
        public string Name { get; set; }
        public List<int> ItemsId {  get; set; }
    }
}
