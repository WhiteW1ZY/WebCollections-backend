using backend.DB;
using backend.Models.Entities.FieldsEntities.BooleanFieldsEntities;
using backend.Models.Fields;
using Microsoft.EntityFrameworkCore;

namespace backend.Service.Fields
{
    public class BooleanFieldService
    {
        private readonly ApplicationContext _context;

        public BooleanFieldService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IResult> GetBooleanFields()
        {
            var fields = await _context.BooleanFields
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

        public async Task<IResult> CreateBooleanField(CreateBooleanFieldEntity createBooleanFieldEntity)
        {
            if (string.IsNullOrEmpty(createBooleanFieldEntity.value.ToString())
                || string.IsNullOrEmpty(createBooleanFieldEntity.name))
            {
                return Results.BadRequest(new { errorText = "value can not be empty" });
            }

            var item = await _context.Items.FirstOrDefaultAsync(item => item.Id == createBooleanFieldEntity.itemId);

            if (item is null)
            {
                return Results.BadRequest(new { errorText = "item with this id is not exist" });
            }

            await _context.BooleanFields.AddAsync(new BooleanField
            {
                item = item,
                Name = createBooleanFieldEntity.name,
                Value = createBooleanFieldEntity.value
            });

            await _context.SaveChangesAsync();

            return Results.Created();
        }

        public async Task<IResult> UpdateBooleanField(int id, UpdateBooleanFieldEntity booleanFieldEntity)
        {
            if (string.IsNullOrEmpty(booleanFieldEntity.value.ToString())
                || string.IsNullOrEmpty(booleanFieldEntity.name))
            {
                return Results.BadRequest(new { errorText = "Values can not be empty" });
            }

            var field = await _context.BooleanFields.FirstOrDefaultAsync(f => f.id == id);

            if (field is null)
            {
                return Results.BadRequest(new { errorText = "Field with this id is not exist" });
            }

            field.Name = booleanFieldEntity.name;
            field.Value = booleanFieldEntity.value;

            await _context.SaveChangesAsync();

            return Results.Ok();
        }

        public async Task<IResult> DeleteBooleanField(int id)
        {
            var field = await _context.BooleanFields.FirstOrDefaultAsync(f => f.id == id);
            if (field is not null)
            {
                _context.BooleanFields.Remove(field);
                await _context.SaveChangesAsync();
            }

            return Results.Ok();
        }
    }
}
