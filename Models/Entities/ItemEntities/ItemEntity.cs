using backend.Models.Fields;

namespace backend.Models.Entities.ItemEntities
{
    public class ItemEntity
    {
        public int Id { get; set; }

        public string Image { get; set; }
        public string Name { get; set; }

        public int CollectionId { get; set; }

        public List<string> Tags { get; set; }
        public List<int> ReactionsId { get; set; }
        public List<int> CommentsId {  get; set; }

        public List<int> BoolFieldsId { get; set; }
        public List<int> IntFieldsId { get; set; }
        public List<int> DateFieldsId { get; set; }
        public List<int> StringFieldsId { get; set; }
    }
}
