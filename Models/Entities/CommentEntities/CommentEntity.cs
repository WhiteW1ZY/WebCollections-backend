namespace backend.Models.Entities.CommentEntities
{
    public class CommentEntity
    {
        public int id { get; set; }
        public string UserName {  get; set; }
        public int ItemId { get; set; }
        public string text { get; set; }
    }
}
