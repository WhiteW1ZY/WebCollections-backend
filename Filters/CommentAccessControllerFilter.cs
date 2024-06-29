using backend.DB;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Filters
{
    public class CommentAccessControllerFilter : Attribute, IAuthorizationFilter
    {
        private readonly ApplicationContext _context;

        public CommentAccessControllerFilter(ApplicationContext context)
        {
            _context = context;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = _context.Users.FirstOrDefault(
                u => u.Login == context.HttpContext.User.Identity.Name);

            if (user is null)
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            int commentId = int.Parse(context.HttpContext.Request.Query["id"]);


            var comment = _context.Comments
                .Include(c => c.Owner)
                .Include(i => i.Item)
                    .ThenInclude(c => c.collection)
                    .ThenInclude(o => o.Owner)
                .FirstOrDefault(c => c.Id == commentId || c.Item.collection.Owner == user);

            if (((comment is null) || (comment.Owner != user)) && !context.HttpContext.User.IsInRole("admin"))
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
        }
    }
}
