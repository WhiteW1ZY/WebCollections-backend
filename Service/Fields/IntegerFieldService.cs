using backend.DB;
using backend.Models.Entities.FieldsEntities.IntegerFieldsValue;
using backend.Models.Fields;
using Microsoft.EntityFrameworkCore;

namespace backend.Service.Fields
{
    public class IntegerFieldService
    {
        private readonly ApplicationContext _context;

        public IntegerFieldService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IResult> GetIntegerFields()
        {
            var fields = await _context.IntegerFields
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

        public async Task<IResult> CreateIntegerField(CreateIntegerFieldEntity createIntegerFieldEntity)
        {
            if (string.IsNullOrEmpty(createIntegerFieldEntity.value.ToString())
                || string.IsNullOrEmpty(createIntegerFieldEntity.name))
            {
                return Results.BadRequest(new { errorText = "value can not be empty" });
            }

            var item = await _context.Items.FirstOrDefaultAsync(item => item.Id == createIntegerFieldEntity.itemId);

            if (item is null)
            {
                return Results.BadRequest(new { errorText = "item with this id is not exist" });
            }

            await _context.IntegerFields.AddAsync(new IntegerField
            {
                item = item,
                Name = createIntegerFieldEntity.name,
                Value = createIntegerFieldEntity.value
            });

            await _context.SaveChangesAsync();

            return Results.Created();
        }

        public async Task<IResult> UpdateIntegerField(int id, UpdateIntegerFieldEntity integerFieldEntity)
        {
            if (string.IsNullOrEmpty(integerFieldEntity.Value.ToString())
                || string.IsNullOrEmpty(integerFieldEntity.Name))
            {
                return Results.BadRequest(new { errorText = "Values can not be empty" });
            }

            var field = await _context.IntegerFields.FirstOrDefaultAsync(f => f.id == id);

            if (field is null)
            {
                return Results.BadRequest(new { errorText = "Field with this id is not exist" });
            }

            field.Name = integerFieldEntity.Name;
            field.Value = integerFieldEntity.Value;

            await _context.SaveChangesAsync();

            return Results.Ok();
        }

        public async Task<IResult> DeleteIntegerField(int id)
        {
            var field = await _context.IntegerFields.FirstOrDefaultAsync(f => f.id == id);
            _context.IntegerFields.Remove(field);
            await _context.SaveChangesAsync();

            return Results.Ok();
        }
    }
}
