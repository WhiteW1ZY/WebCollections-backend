namespace backend.Models.Entities.CommentEntities
{
    public class CreateCommentEntity
    {
        public string UserName { get; set; }
        public int ItemId { get; set; }
        public string text { get; set; }
    }
}
