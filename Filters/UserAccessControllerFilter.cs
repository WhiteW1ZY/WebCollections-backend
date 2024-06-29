using backend.DB;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace backend.Filters
{
    public class UserAccessControllerFilter: Attribute, IAuthorizationFilter
    {
        private readonly ApplicationContext _context;

        public UserAccessControllerFilter(ApplicationContext context)
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
                return;
            }

            var userLogin= context.HttpContext.Request.Query["login"];

            if ( userLogin != user.Login && !context.HttpContext.User.IsInRole("admin"))
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
        }
    }
}
