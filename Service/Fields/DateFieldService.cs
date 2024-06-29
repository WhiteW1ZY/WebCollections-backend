using backend.DB;
using backend.Models.Entities.FieldsEntities.DateFieldsEntities;
using backend.Models.Fields;
using Microsoft.EntityFrameworkCore;

namespace backend.Service.Fields
{
    public class DateFieldService
    {
        private readonly ApplicationContext _context;

        public DateFieldService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IResult> GetFields()
        {
            var fields = await _context.DateFields
                .Include(f => f.item)
                .ToListAsync();

            return Results.Ok(
                fields.Select(field => new
                {
                    id = field.id,
                    Name = field.Name,
                    Day = field.Day,
                    Month = field.Month,
                    Year = field.Year,
                    itemId = field.item.Id
                })
                );
        }

        public async Task<IResult> CreateDateField(CreateDateFieldEntity createDateFieldEntity)
        {
            if (string.IsNullOrEmpty(createDateFieldEntity.name)
                
                || ( string.IsNullOrEmpty(createDateFieldEntity.Year.ToString())
                     && string.IsNullOrEmpty(createDateFieldEntity.Month.ToString())
                     && string.IsNullOrEmpty(createDateFieldEntity.Day.ToString())
                     )
                )
            {
                return Results.BadRequest(new { errorText = "value can not be empty" });
            }

            var item = await _context.Items.FirstOrDefaultAsync(item => item.Id == createDateFieldEntity.itemId);

            if (item is null)
            {
                return Results.BadRequest(new { errorText = "item with this id is not exist" });
            }

            await _context.DateFields.AddAsync(new DateField
            {
                item = item,
                Name = createDateFieldEntity.name,
                Day = createDateFieldEntity.Day,
                Month = createDateFieldEntity.Month,
                Year = createDateFieldEntity.Year
            });

            await _context.SaveChangesAsync();

            return Results.Created();
        }

        public async Task<IResult> UpdateDateField(int id, UpdateDateFieldEntity dateFieldEntity)
        {
            if (string.IsNullOrEmpty(dateFieldEntity.name)

                || (string.IsNullOrEmpty(dateFieldEntity.Year.ToString())
                     && string.IsNullOrEmpty(dateFieldEntity.Month.ToString())
                     && string.IsNullOrEmpty(dateFieldEntity.Day.ToString())
                     )
                )
            {
                return Results.BadRequest(new { errorText = "value can not be empty" });
            }

            var field = await _context.DateFields.FirstOrDefaultAsync(f => f.id == id);

            if (field is null)
            {
                return Results.BadRequest(new { errorText = "Field with this id is not exist" });
            }

            field.Day = dateFieldEntity.Day;
            field.Month = dateFieldEntity.Month;
            field.Year = dateFieldEntity.Year;
            field.Name = dateFieldEntity.name;

            await _context.SaveChangesAsync();

            return Results.Ok();
        }

        public async Task<IResult> DeleteDateField(int id)
        {
            var field = await _context.DateFields.FirstOrDefaultAsync(f => f.id == id);
            if (field is not null)
            {
                _context.DateFields.Remove(field);
                await _context.SaveChangesAsync();
            }

            return Results.Ok();
        }
    }
}
