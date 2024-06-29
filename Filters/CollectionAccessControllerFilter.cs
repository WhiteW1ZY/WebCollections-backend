using backend.DB;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace backend.Filters
{
    public class CollectionAccessControllerFilter: Attribute, IAuthorizationFilter
    {
        private readonly ApplicationContext _context;

        public CollectionAccessControllerFilter(ApplicationContext context)
        {
            _context = context;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = _context.Users.FirstOrDefault(
                u => u.Login == context.HttpContext.User.Identity.Name);

            if( user is null )
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            var collectionId = int.Parse(context.HttpContext.Request.Query["id"]);

            var collection = _context.Collections.Include(o => o.Owner).FirstOrDefault(c => c.Id == collectionId);

            if ((collection is null || collection.Owner != user) && !context.HttpContext.User.IsInRole("admin"))
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

        }
    }

}
