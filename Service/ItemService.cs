using backend.DB;
using backend.Models;
using backend.Models.Entities.ItemEntities;
using backend.Models.Entities.TagEntities;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;

namespace backend.Service
{
    public class ItemService
    {
        private readonly ApplicationContext _context;
        private readonly TagService _tagService;

        public ItemService(ApplicationContext context)
        {
            _context = context;
            _tagService = new TagService(context);
        }

        public async Task<IResult> GetItems()
        {
            var items = await _context.Items
                .Include(itemFields => itemFields.DateFields)
                .Include(itemFields => itemFields.BooleanFields)
                .Include(itemFields => itemFields.IntegerFields)
                .Include(itemFields => itemFields.StringFields)
                .Include(item => item.Comments)
                .Include(item => item.Reactions)
                .Include(item => item.Tags)
                .Include(item => item.collection)
                .ToListAsync();

            return Results.Ok(
                items.Select(item => new ItemEntity
                {
                    Id = item.Id,
                    CollectionId = item.collection.Id,
                    BoolFieldsId = item.BooleanFields.Select(field => field.id).ToList(),
                    DateFieldsId = item.DateFields.Select(field => field.id).ToList(),
                    IntFieldsId = item.IntegerFields.Select(field => field.id).ToList(),
                    StringFieldsId = item.StringFields.Select(field => field.id).ToList(),
                    ReactionsId = item.Reactions.Select(reaction => reaction.Id).ToList(),
                    CommentsId = item.Comments.Select(comment => comment.Id).ToList(),
                    Image = item.Image,
                    Name = item.Name,
                    Tags = item.Tags.Select(tag => tag.TagName).ToList()
                }));
        }

        public async Task<IResult> CreateItem(ItemCreateEntity itemEntity)
        {
            if (string.IsNullOrEmpty(itemEntity.Name))
            {
                return Results.BadRequest(new { errorText = "Item must be named" });
            }

            var collection = await _context.Collections.FirstOrDefaultAsync(c => c.Id == itemEntity.CollectionId);

            if (collection is null) 
            {
                return Results.BadRequest(new { errorText = "Collection with this id is not exist" });
            }

            foreach (var tag in itemEntity.Tags)
            {
                await _tagService.CreateTag(new TagCreateEntity { TagName = tag });
            }

            var tags = await _context.tags.Where(t => itemEntity.Tags.Contains(t.TagName)).ToListAsync();

            var filePath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\", itemEntity.Image.FileName));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await itemEntity.Image.CopyToAsync(stream);
            }

            await _context.Items.AddAsync(new Item
            {
                Image = itemEntity.Image.FileName,
                collection = collection,
                Name = itemEntity.Name,
                Tags = tags
            });
            await _context.SaveChangesAsync();

            return Results.Created();
        }

        public async Task<IResult> UpdateItem(int id, ItemUpdateEntity itemEntity)
        {
            if (string.IsNullOrEmpty(itemEntity.Name))
            {
                return Results.BadRequest(new { errorText = "Item must be named" });
            }

            var item = await _context.Items.Include(i => i.Tags).FirstOrDefaultAsync(item => item.Id == id);

            item.Tags = new List<Tag>();

            if(item is null)
            {
                return Results.BadRequest(new { errorText = "Item with this id is not exist" });
            }

            foreach (var tag in itemEntity.Tags)
            {
                await _tagService.CreateTag(new TagCreateEntity { TagName = tag });
            }

            var tags = await _context.tags.Where(t => itemEntity.Tags.Contains(t.TagName)).ToListAsync();

            var filePath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\", itemEntity.Image.FileName));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await itemEntity.Image.CopyToAsync(stream);
            }
            item.Name = itemEntity.Name;
            item.Image = itemEntity.Image.FileName;
            item.Tags = tags;

            await _context.SaveChangesAsync();

            return Results.Ok();
        }

        public async Task<IResult> DeleteItem(int id)
        {
            var item = await _context.Items
                .Include(c => c.Comments)
                .Include(r => r.Reactions)
                .FirstOrDefaultAsync(item => item.Id == id);

            if (item is not null)
            {
                
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }

            return Results.Ok();
        }
    }
}
