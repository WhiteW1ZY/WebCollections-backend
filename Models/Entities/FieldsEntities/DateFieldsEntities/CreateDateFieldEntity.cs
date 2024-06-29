namespace backend.Models.Entities.FieldsEntities.DateFieldsEntities
{
    public class CreateDateFieldEntity
    {
        public string name { get; set; }

        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        
        public int itemId { get; set; }
    }
}
