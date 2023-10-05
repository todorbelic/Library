using BookStoreAPI.Exceptions;

namespace BookStoreAPI.Middleware
{
    public class ExceptionMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                httpContext.Response.StatusCode = (int)ExceptionStatusCode.GetExceptionStatusCode(ex);
                httpContext.Response.ContentType = "text/plain";
                await httpContext.Response.WriteAsync(ex.Message);
            }
        }
    }
}
