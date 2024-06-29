using backend.DB;
using backend.Models;
using backend.Models.Entities.CollectionEntities;
using Microsoft.EntityFrameworkCore;

namespace backend.Service
{
    public class CollectionService
    {

        private readonly ApplicationContext _context;

        public CollectionService(ApplicationContext context)
        {
            _context = context;
        }


        public async Task<IResult> GetCollections()
        {
            var collections = await _context.Collections
                .Include(c => c.Owner)
                .Include(c => c.Items)
            .ToListAsync();

            return Results.Ok(

               collections.Select(collection => new CollectionEntity
               {
                   Id = collection.Id,
                   Name = collection.Name,
                   Image = collection.Image,
                   ItemsId = collection.Items.Select(item => item.Id).ToList(),
                   OwnerId = collection.Owner.id
               }).ToList()
               );
        }

        public async Task<IResult> CreateCollection(CreateCollectionEntity collectionEntity)
        {
            if (string.IsNullOrEmpty(collectionEntity.Name))
            {
                return Results.BadRequest(new { errorText = "Value can not be empty" });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.id == collectionEntity.OwnerId);

            if (user is null) 
            {
                return Results.BadRequest(new { errorText = "User with this id is not exist" });
            }
            
            var filePath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\",collectionEntity.Image.FileName));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await collectionEntity.Image.CopyToAsync(stream);
            }

            await _context.Collections.AddAsync(new Collection
            {
                Name = collectionEntity.Name,
                Image = collectionEntity.Image.FileName,
                Owner = user,
                Items = new List<Item>()
            });

            await _context.SaveChangesAsync();

            return Results.Created();
        }

        public async Task<IResult> UpdateCollection(UpdateCollectionEntity collectionEntity)
        {
            if (string.IsNullOrEmpty(collectionEntity.Name))
            {
                return Results.BadRequest(new { errorText = "Value can not be empty" });
            }

            var collection = await _context.Collections.FirstOrDefaultAsync(c => c.Id == collectionEntity.id);

            if(collection is null)
            {
                return Results.BadRequest(new { errorText = "Collection with this id is not exist" });
            }
           
            collection.Name = collectionEntity.Name;

            var filePath = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\", collectionEntity.Image.FileName));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await collectionEntity.Image.CopyToAsync(stream);
            }

            collection.Image = collectionEntity.Image.FileName;

            await _context.SaveChangesAsync();

            return Results.Ok();
        }

        public async Task<IResult> DeleteCollection(int id)
        {
            var collection = await _context.Collections
                .Include(i => i.Items)
                    .ThenInclude(r => r.Reactions)
                .Include(i => i.Items)
                    .ThenInclude(c => c.Comments)
                .FirstOrDefaultAsync(c => c.Id == id);
            
            if (collection is not null)
            {
                _context.Collections.Remove(collection);
                await _context.SaveChangesAsync();
            }

            return Results.Ok();
        }
    }
}
