using Microsoft.AspNetCore.Mvc.Filters;
using WebDating.Extensions;
using WebDating.Interfaces;

namespace WebDating.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userId = resultContext.HttpContext.User.GetUserId();

            var uow = resultContext.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();

            var user = await uow.UserRepository.GetUserByIdAsync(userId);

            user.LastActive = DateTime.Now;

            await uow.Complete();

        }
    }
}
