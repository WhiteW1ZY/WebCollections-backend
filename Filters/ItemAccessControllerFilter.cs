using backend.DB;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace backend.Filters
{
    public class ItemAccessControllerFilter : Attribute, IAuthorizationFilter
    {
        private readonly ApplicationContext _context;

        public ItemAccessControllerFilter(ApplicationContext context)
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

            var itemId = int.Parse(context.HttpContext.Request.Query["id"]);

            var collection = _context.Collections.FirstOrDefault(c => c.Items.Select(i => i.Id).Contains(itemId));

            if ((collection is null || collection.Owner is null || collection.Owner.id != user.id) && !context.HttpContext.User.IsInRole("admin"))
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
        }
    }
}
