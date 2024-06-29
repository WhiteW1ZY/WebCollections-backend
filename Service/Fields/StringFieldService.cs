using backend.DB;
using Microsoft.EntityFrameworkCore;
using backend.Models.Entities.FieldsEntities.StringFieldsEntities;
using backend.Models.Fields;

namespace backend.Service.Fields
{
    public class StringFieldService
    {
        private readonly ApplicationContext _context;

        public StringFieldService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IResult> GetStringFields()
        {
            var fields = await _context.StringFields
                .Include(f => f.item)
                .ToListAsync();

            return Results.Ok(
                fields.Select(field => new
                {
                    id = field.id,
                    Name = field.Name,
                    Value = field.Value,
                    itemId = field.item.Id
                })
                );
        }

        public async Task<IResult> CreateStringField(CreateStringFieldEntity createStringFieldEntity)
        {
            if (string.IsNullOrEmpty(createStringFieldEntity.value)
                || string.IsNullOrEmpty(createStringFieldEntity.name))
            {
                return Results.BadRequest(new { errorText = "value can not be empty" });
            }

            var item = await _context.Items.FirstOrDefaultAsync(item => item.Id == createStringFieldEntity.itemId);

            if(item is null)
            {
                return Results.BadRequest(new { errorText = "item with this id is not exist" });
            }

            await _context.StringFields.AddAsync(new StringField { 
                item = item, 
                Name = createStringFieldEntity.name,
                Value = createStringFieldEntity.value });

            await _context.SaveChangesAsync();

            return Results.Created();
        }

        public async Task<IResult> UpdateStringField(int id, UpdateStringFieldEntity stringFieldEntity)
        {
            if (string.IsNullOrEmpty(stringFieldEntity.Value)
                || string.IsNullOrEmpty(stringFieldEntity.Name))
            {
                return Results.BadRequest(new { errorText = "Values can not be empty" });
            }

            var field = await _context.StringFields.FirstOrDefaultAsync(f => f.id == id);

            if (field is null)
            {
                return Results.BadRequest(new { errorText = "Field with this id is not exist" });
            }

            field.Name = stringFieldEntity.Name;
            field.Value = stringFieldEntity.Value;

            await _context.SaveChangesAsync();

            return Results.Ok();
        }

        public async Task<IResult> DeleteStrignField(int id)
        {
            var field = await _context.StringFields.FirstOrDefaultAsync(f => f.id == id);
            _context.StringFields.Remove(field);
            await _context.SaveChangesAsync();

            return Results.Ok();
        }
    }
}
