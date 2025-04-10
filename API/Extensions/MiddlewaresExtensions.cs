using API.Middleware;

namespace API.Extensions;

public static class MiddlewaresExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
        => app.UseMiddleware<ExceptionMiddleware>();
}
