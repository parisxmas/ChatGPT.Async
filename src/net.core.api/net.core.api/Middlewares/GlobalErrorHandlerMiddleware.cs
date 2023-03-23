using System.Net;
using System.Text;
using net.core.data.Exceptions;

namespace net.core.api.Middlewares;

public class GlobalErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalErrorHandlerMiddleware> _logger;

    public GlobalErrorHandlerMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            var request = context.Request;
            HttpRequestRewindExtensions.EnableBuffering(request);
            var requestBody = string.Empty;


            using (StreamReader reader = new StreamReader(
                       request.Body,
                       Encoding.UTF8,
                       detectEncodingFromByteOrderMarks: false,
                       leaveOpen: true))
            {
                // IMPORTANT: Reset the request body stream position so the next middleware can read it
                request.Body.Position = 0;
                requestBody = await reader.ReadToEndAsync();
            }


            _logger.LogError(error,
                $"Parametreler: Host: {context.Request.Host} - Controller: {context.Request.RouteValues["controller"]} - Action: {context.Request.RouteValues["action"]} - Path: {context.Request.Path} - Method: {context.Request.Method} - requestBody: {requestBody} - Custom Message: {error.Message}");

            switch (error)
            {
                case AppException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            
            // context.Response.Redirect("/Home/Error");
        }
    }
}