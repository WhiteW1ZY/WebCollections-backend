using backend.DB;
using backend.Models;
using backend.Models.Entities.TagEntities;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class TagService
    {
        private readonly ApplicationContext _context;

        public TagService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IResult> GetTags()
        {
            var tags = await _context.tags
                   .Include(items => items.Items)
                   .ToListAsync();

            return Results.Ok(
                tags.Select(tag => new TagEntity
                {
                    TagId = tag.Id,
                    TagName = tag.TagName,
                    ItemsId = tag.Items.Select(t => t.Id).ToList(),
                }).ToList()
                );
        }

        public async Task<IResult> CreateTag(TagCreateEntity tagEntity)
        {
            if( string.IsNullOrEmpty(tagEntity.TagName) 
                || tagEntity.TagName.Length > 20)
                {
                return Results.BadRequest(new { errorText = "The tag name cannot be empty or contain more than 20 characters." });
            }
            var tagElement = await _context.tags.FirstOrDefaultAsync(t => t.TagName == tagEntity.TagName);

            if (tagElement is null)
            { 
                var tag = new Tag { TagName = tagEntity.TagName };
                tag.Items = await _context.Items.Where(item => tagEntity.ItemsId.Contains(item.Id)).ToListAsync();

                await _context.tags.AddAsync(tag);
                await _context.SaveChangesAsync();
            }

            return Results.Created();
        }

        public async Task<IResult> UpdateTag(string tagName, TagUpdateEntity tagEntity)
        {
            if ( string.IsNullOrEmpty(tagEntity.TagName)
                || tagEntity.TagName.Length > 20)
            {
                return Results.BadRequest(new { errorText = "The tag name cannot be empty or contain more than 20 characters." });
            }

            var tag = await _context.tags.FirstOrDefaultAsync(t => t.TagName == tagName);

            if (tag is null)
            {
                return Results.BadRequest(new { errorText = "Tag with this name is not exist" });
            }

            tag.TagName = tagEntity.TagName;
            tag.Items = await _context.Items.Where(item => tagEntity.ItemsId.Contains(item.Id)).ToListAsync();

            await _context.SaveChangesAsync();
            return Results.Ok();
        }

        public async Task<IResult> DeleteTag(string name)
        {
            var tag = await _context.tags.FirstOrDefaultAsync(t => t.TagName == name);
            if(tag is not null)
            {
                _context.tags.Remove(tag);
                await _context.SaveChangesAsync();
            }

            return Results.Ok();
        }
        
    }
}
