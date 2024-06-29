using System.ComponentModel.DataAnnotations;

namespace backend.Models.Fields
{
    public class StringField
    {
        [Key]
        public int id { get; set; }
        public Item item { get; set; }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}
