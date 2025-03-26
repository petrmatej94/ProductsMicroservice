using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Products.Application.Services;
using Products.Application.Validators;
using Products.Persistence;
using Products.Persistence.Database;
using System.Reflection;
using Products.Api.Middlewares;
using Serilog;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Products.Application.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) =>
	loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
	var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
	var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
	options.IncludeXmlComments(xmlPath);

	var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

	foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
	{
		options.SwaggerDoc(description.GroupName, new OpenApiInfo
		{
			Title = $"Products API {description.ApiVersion}",
			Version = description.ApiVersion.ToString()
		});
	}
});

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();

builder.Services.AddApiVersioning(options =>
{
	options.AssumeDefaultVersionWhenUnspecified = true;
	options.DefaultApiVersion = new ApiVersion(1, 0);
	options.ReportApiVersions = true;
	options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddVersionedApiExplorer(options =>
{
	options.GroupNameFormat = "'v'VVV";
	options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();


var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

var swaggerEnabled = builder.Configuration.GetValue<bool>("Swagger:Enabled");
if (swaggerEnabled)
{
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
		foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
		{
			options.SwaggerEndpoint(
				$"/swagger/{description.GroupName}/swagger.json",
				description.GroupName.ToUpperInvariant());
		}
	});
}

app.UseHttpsRedirection();

app.MapControllers();

using (IServiceScope scope = app.Services.CreateScope())
{
	ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    ApplicationDbContext.Seed(dbContext);
}

try
{
	Log.Information("Starting Products REST API in Environment: {Environment}", app.Environment.EnvironmentName);
	app.Run();
}
catch (Exception ex)
{
	Log.Fatal(ex, "App failed during startup.");
}
finally
{
	Log.CloseAndFlush();
}
