using backend.DB;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using backend.Models.Entities.CommentEntities;

namespace backend.Service
{
    public class CommentService
    {
        private readonly ApplicationContext _context;

        public CommentService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IResult> GetComments()
        {
            var comments = await _context.Comments
                .Include(c => c.Owner)
                .Include(c => c.Item)
                .ToListAsync();

            if (comments is null)
            {
                return Results.NoContent();
            }

            return Results.Ok(
                comments.Select(comment => new CommentEntity
                {
                    id = comment.Id,
                    ItemId = comment.Item.Id,
                    text = comment.Text,
                    UserName = comment.Owner.Login
                }).ToList()
                );
        }

        public async Task<IResult> CreateComment(CreateCommentEntity createComment)
        {
            if (string.IsNullOrEmpty(createComment.text))
            {
                return Results.BadRequest(new { errorText = "text can not be empty" });
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == createComment.UserName);

            if(user is null)
            {
                return Results.BadRequest(new { errorText = "user with this name is not exist" });
            }

            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == createComment.ItemId);
            
            if(item is null)
            {
                return Results.BadRequest(new { errorText = "item with this id is not exist" });
            }

            await _context.Comments.AddAsync(new Comment { Item = item, Owner = user, Text = createComment.text });
            await _context.SaveChangesAsync();

            return Results.Created();
        }

        public async Task<IResult> UpdateComment(int commentId, CommentUpdateEntity commentEntity)
        {
            if (string.IsNullOrEmpty(commentEntity.text))
            {
                return Results.BadRequest(new { errorText = "text can not be empty" });
            }

            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
            if(comment is null)
            {
                return Results.BadRequest(new { errorText = "comment with this id is not exist" });
            }

            comment.Text = commentEntity.text;

            await _context.SaveChangesAsync();

            return Results.Ok();
        }

        public async Task<IResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if(comment is not null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }

            return Results.Ok();
        }
    }
}
