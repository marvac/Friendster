using Friendster.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Friendster.Helpers
{
    public class LogActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                var resultContext = await next();
                var userId = int.Parse(resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var repo = resultContext.HttpContext.RequestServices.GetService<IFriendRepository>();

                var user = await repo.GetUser(userId);
                user.LastActive = DateTime.Now;
                await repo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
