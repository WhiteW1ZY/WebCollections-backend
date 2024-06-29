namespace backend.Models.Entities.ReactionEntities
{
    public class CreateReactionEntity
    {
        public string UserName { get; set; }
        public int ItemId { get; set; }
        public bool isLike { get; set; }
    }
}
