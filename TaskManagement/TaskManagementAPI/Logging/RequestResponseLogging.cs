using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TaskManagementAPI.Interfaces;

namespace TaskManagementAPI.Logging
{
    public class RequestResponseLogging
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;

        public RequestResponseLogging(RequestDelegate next, ILoggerManager loggerManager)
        {
            _next = next;
            _logger = loggerManager;
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.LogInfo(FormatRequest(context.Request));

            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await _next(context);

                _logger.LogInfo(FormatResponse(context.Response));

                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private string FormatRequest(HttpRequest request)
        {

            var time = DateTime.Now.ToString();
            return $"[REQUEST INFO] Time: {time} Method: {request.Method} Scheme: {request.Scheme} CotentType: {request.ContentType} Target: {request.Host}{request.Path}{request.QueryString}";
        }

        private string FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var time = DateTime.Now.ToString();
            return $"[RESPONSE INFO] Time: {time} Status code: {response.StatusCode} ContentType: {response.ContentType} HttpContext: {response.HttpContext}";
        }
    }
}