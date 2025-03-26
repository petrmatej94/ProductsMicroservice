using System.Diagnostics;

namespace Products.Api.Middlewares;

public class RequestLoggingMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<RequestLoggingMiddleware> _logger;

	public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		_logger.LogInformation("Incoming request: {Method} {Path}", context.Request.Method, context.Request.Path);

		var stopwatch = Stopwatch.StartNew();

		await _next(context);

		stopwatch.Stop();

		_logger.LogInformation("Request {Method} {Path} responded code: {StatusCode} in {Elapsed} ms", 
			context.Request.Method, context.Request.Path, context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
	}
}
