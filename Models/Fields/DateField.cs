using System.ComponentModel.DataAnnotations;

namespace backend.Models.Fields
{
    public class DateField
    {
        [Key]
        public int id { get; set; }
        public Item item { get; set; }

        public string Name { get; set; }

        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
