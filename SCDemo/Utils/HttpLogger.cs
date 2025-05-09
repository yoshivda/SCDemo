namespace SCDemo;

using System.Diagnostics;

public class HttpLogger(RequestDelegate next, ILogger<HttpLogger> logger)
{
    public async Task Invoke(HttpContext context)
    {
        logger.LogInformation("Request: {Method} {Path} started.", context.Request.Method, context.Request.Path);

        var timer = Stopwatch.StartNew();
        await next(context);
        timer.Stop();

        var stats = context.RequestServices.GetService<QueryStats>();
        logger.LogInformation("Request: {Method} {Path} finished. Response: code {Code}, duration {ResponseTime}ms. DB: retrieved {Rows} rows using {Queries} queries in {DbTime}ms.",
            context.Request.Method, context.Request.Path, context.Response.StatusCode, timer.ElapsedMilliseconds, stats.Rows, stats.Queries, stats.Duration.Milliseconds);
    }
}
