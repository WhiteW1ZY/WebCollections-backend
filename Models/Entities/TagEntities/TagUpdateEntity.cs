namespace backend.Models.Entities.TagEntities
{
    public class TagUpdateEntity
    {
        public string TagName { get; set; }
        public List<int> ItemsId { get; set; }
    }
}
