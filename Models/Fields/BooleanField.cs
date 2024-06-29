using System.ComponentModel.DataAnnotations;

namespace backend.Models.Fields
{
    public class BooleanField
    {
        [Key]
        public int id { get; set; }

        public Item item { get; set; }

        public string Name { get; set; }

        public bool Value { get; set; }
    }
}
