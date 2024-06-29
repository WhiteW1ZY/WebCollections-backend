namespace backend.Models.Entities.TagEntities
{
    public class TagEntity
    {
        public int TagId { get; set; }

        public string TagName { get; set; }
            
        public List<int> ItemsId { get; set; }
    }
}
