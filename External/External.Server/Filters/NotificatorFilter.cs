using System;
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
                context.HttpContext.Response.StatusCode = _notificator.NotificationType switch
                {
                    NotificationType.Internal => (int)HttpStatusCode.InternalServerError,
                    NotificationType.Validation => (int)HttpStatusCode.BadRequest,
                    NotificationType.Authentication => (int)HttpStatusCode.Unauthorized,
                    NotificationType.NotFound => (int)HttpStatusCode.NotFound,
                    _ => (int)HttpStatusCode.InternalServerError
                };
                context.HttpContext.Response.ContentType = "application/json";
                var notifications = await Task.Factory.StartNew(() => JsonConvert.SerializeObject(_notificator.Notifications));
                await context.HttpContext.Response.WriteAsync(notifications);
                return;
            }

            await next();
        }
    }
}
