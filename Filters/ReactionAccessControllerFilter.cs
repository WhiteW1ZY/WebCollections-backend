using backend.DB;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace backend.Filters
{
    public class ReactionAccessControllerFilter : Attribute, IAuthorizationFilter
    {
        private readonly ApplicationContext _context;

        public ReactionAccessControllerFilter(ApplicationContext context)
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

            var reactionId = int.Parse(context.HttpContext.Request.Query["id"]);

            var reaction = _context.Reactions.FirstOrDefault(r => r.Id == reactionId);

            if ((reaction is null || reaction.Owner.id != user.id) && !context.HttpContext.User.IsInRole("admin"))
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
        }
    }
}
