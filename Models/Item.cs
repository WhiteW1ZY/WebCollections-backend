using backend.Models.Fields;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        public Collection collection { get; set; }

        public string Image { get; set; }
        public string Name { get; set; }

        public List<Tag> Tags { get; set; }

        public List<Comment> Comments { get; set; }
        public List<Reaction> Reactions { get; set; }

        public List<StringField> StringFields { get; set; }
        public List<IntegerField> IntegerFields { get; set; }
        public List<BooleanField> BooleanFields { get; set; }
        public List<DateField> DateFields { get; set; }

    }
}
