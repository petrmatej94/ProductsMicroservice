using Microsoft.AspNetCore.Mvc;

namespace Products.Api.Middlewares;

public class ExceptionHandlingMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<ExceptionHandlingMiddleware> _logger;

	public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Unhandled exception occurred.");

			context.Response.ContentType = "application/problem+json";
			context.Response.StatusCode = StatusCodes.Status500InternalServerError;

			var problem = new ProblemDetails
			{
				Status = StatusCodes.Status500InternalServerError,
				Title = "Unexpected error occurred",
				Detail = ex.Message,
				Instance = context.Request.Path
			};

			await context.Response.WriteAsJsonAsync(problem);
		}
	}
}
