using Microsoft.AspNetCore.Builder;

namespace TaskManagementAPI.Logging
{
    public static class RequestResponseLogginExtension
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLogging>();
        }
    }
}