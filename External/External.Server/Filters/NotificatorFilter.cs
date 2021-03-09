using System.Net;
using System.Threading.Tasks;
using Core.Shared.Notifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace External.Server.Filters
{
    public class NotificatorFilter : IAsyncResultFilter
    {
        private readonly INotificator _notificator;

        public NotificatorFilter (INotificator notificator)
        {
            _notificator = notificator;
        }

        public async Task OnResultExecutionAsync (ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (_notificator.HasNotifications)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.HttpContext.Response.ContentType = "application/json";
                var notifications = JsonConvert.SerializeObject(_notificator.Notifications);
                await context.HttpContext.Response.WriteAsync(notifications);
                return;
            }

            await next();
        }
    }
}
