namespace backend.Models.Entities.ReactionEntities
{
    public class ReactionEntity
    {
        public int id { get; set; }
        public string UserName { get; set; }
        public int ItemId { get; set; }
        public bool isLiked { get; set; }
    }
}
