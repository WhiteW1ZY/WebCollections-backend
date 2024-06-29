using backend.DB;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Filters.FieldFilters
{
    public class IntegerFieldAccessControllerFilter: Attribute, IAuthorizationFilter
    {
        private readonly ApplicationContext _context;

        public IntegerFieldAccessControllerFilter(ApplicationContext context)
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

            var fieldId = int.Parse(context.HttpContext.Request.Query["id"]);

            var field = _context.IntegerFields
                .Include(i => i.item)
                .ThenInclude(c => c.collection)
                .ThenInclude(o => o.Owner)
                .FirstOrDefault(field => field.id == fieldId);

            if ((field is null || field.item.collection.Owner != user) && !context.HttpContext.User.IsInRole("admin"))
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

        }
    }
}
