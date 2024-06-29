using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Tag
    {
      
        [Key]
        public int Id { get; set; }
        public string TagName { get; set; }

        public List<Item> Items { get; set; }
    }
}
