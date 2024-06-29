namespace backend.Models.Entities.TagEntities
{
    public class TagCreateEntity
    {
        public string TagName { get; set; }

        public List<int> ItemsId { get; set; }
    }
}
