using backend.DB;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using backend.Models.Entities.ReactionEntities;
using System.Xml.Linq;

namespace backend.Service
{
    public class ReactionService
    {
        private readonly ApplicationContext _context;

        public ReactionService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IResult> GetReactions()
        {
            var reactions = await _context.Reactions
                .Include(r => r.Owner)
                .Include(r => r.Item)
            .ToListAsync();

            if (reactions is null)
            {
                return Results.NoContent();
            }

            return Results.Ok(
                reactions.Select(reaction => new ReactionEntity
                {
                    id = reaction.Id,
                    ItemId = reaction.Item.Id,
                    isLiked = reaction.IsLike,
                    UserName = reaction.Owner.Login
                }).ToList()
                );
        }

        public async Task<IResult> CreateReaction(CreateReactionEntity createReaction)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == createReaction.UserName);

            if (user is null)
            {
                return Results.BadRequest(new { errorText = "user with this name is not exist" });
            }

            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == createReaction.ItemId);

            if (item is null)
            {
                return Results.BadRequest(new { errorText = "item with this id is not exist" });
            }

            var IsExist = await _context.Reactions.FirstOrDefaultAsync(r => r.Owner == user && r.Item == item);

            if(IsExist is not null) 
            {
                return Results.BadRequest(new { errorText = "User with this id is already reacted on this item" });
            }

            await _context.Reactions.AddAsync(new Reaction { Item = item, Owner = user, IsLike = createReaction.isLike });
            await _context.SaveChangesAsync();

            return Results.Created();
        }

        public async Task<IResult> UpdateReaction(int reactionId, UpdateReactionEntity reactionEntity)
        {

            var reaction = await _context.Reactions.FirstOrDefaultAsync(r => r.Id == reactionId);
            if (reaction is null)
            {
                return Results.BadRequest(new { errorText = "comment with this id is not exist" });
            }

            reaction.IsLike = reactionEntity.isLike;

            await _context.SaveChangesAsync();

            return Results.Ok();
        }

        public async Task<IResult> DeleteReaction(int id)
        {
            var reaction = await _context.Reactions.FirstOrDefaultAsync(r => r.Id == id);
            if (reaction is not null)
            {
                _context.Reactions.Remove(reaction);
                await _context.SaveChangesAsync();
            }

            return Results.Ok();
        }
    }
}
