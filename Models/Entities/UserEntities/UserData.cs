namespace backend.Models.Entityes.UserEntityes
{
    public class UserData
    {
        public int id {  get; set; }

        public List<int> CollectionsId { get; set; }
        public List<int> ReactionsId { get; set; }
        public List<int> CommentsId { get; set; }

        public string login { get; set; }
        public string role { get; set; }
        public bool isBanned { get; set; }
    }
}
