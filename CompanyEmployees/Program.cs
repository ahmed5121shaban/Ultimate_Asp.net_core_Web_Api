using CompanyEmployees;
using CompanyEmployees.Extensions;
using CompanyEmployees.Mapping;
using Contracts;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NLog;
using AspNetCoreRateLimit;
using MediatR;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Identity;
using FluentValidation;
using Application.Behaviors;

var builder = WebApplication.CreateBuilder(args);

// Nlog Configuration
LogManager.Setup().LoadConfigurationFromFile(Path.Combine(Directory.GetCurrentDirectory(), "nlog.config"));

// Add services to the container.
builder.Services.AddControllers();

// Cors Configuration
builder.Services.ConfigureCors();

// Logger Service Configuration
builder.Services.ConfigureLoggerService();

// Repository Manager Configuration
builder.Services.ConfigureRepositoryManager();

// Filter Configuration
builder.Services.ConfigurationFilter();

// Service Manager Configuration
builder.Services.ConfigureServiceManager();

// DataShaper Configuration For EmployeeDto
builder.Services.DataShaperEmployeeDtoConfiguration();

// Employee Links Configuration
builder.Services.EmployeeLinksConfig();

// Configure Api Versioning
builder.Services.ConfigureVersioning();

// add the assemply file for mediator
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.AssemblyReference).Assembly));

// Configure Response Caching
builder.Services.ConfigureResponseCaching();

// Configure Http Cache Headers
builder.Services.ConfigureHttpCacheHeaders();

// SqlContex Configuration
builder.Services.ConfigureSqlContext(builder.Configuration);

// CQRS Piprline Validator
builder.Services.AddTransient(typeof(IPipelineBehavior<,>),typeof(ValidationBehavior<,>));

// Add Flount Validators
builder.Services.AddValidatorsFromAssembly(typeof(Application.AssemblyReference).Assembly);

// Add Controllers Project With Config
builder.Services.AddControllers(config =>
{
    config.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
    config.CacheProfiles.Add("120SecondsDuration", new CacheProfile{Duration =120});

}).AddNewtonsoftJson()
.AddApplicationPart(typeof(CompanyEmployees.Presentation.AssemblyReference).Assembly);


// read secret key from hte environment
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();


//Add Custom Media Types for Hiper Media
builder.Services.AddCustomMediaTypes();

// Validate MediaType Attribute Configuration
builder.Services.ValidateMediaTypeAttributeConfig();

// Add Memory Cache Service
builder.Services.AddMemoryCache();

// Configure RateLimiting Options
builder.Services.ConfigureRateLimitingOptions();

// Add Http Context Accessor
builder.Services.AddHttpContextAccessor();

// Register Mapster mappings
MapsterConfig.RegisterMappings();

// Add Authentication Configuration
builder.Services.AddAuthentication();

// Identity Configuration
builder.Services.ConfigureIdentity();

// JWT Configuration
builder.Services.ConfigureJWT(builder.Configuration);

// Add Jwt Configuration
builder.Services.AddJwtConfiguration(builder.Configuration);

// Swagger Configuration
builder.Services.ConfigureSwagger();

// Add Json Patch Format Configuration
NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter() => new ServiceCollection().AddLogging().AddMvc().AddNewtonsoftJson()
    .Services.BuildServiceProvider()
    .GetRequiredService<IOptions<MvcOptions>>().Value.InputFormatters
    .OfType<NewtonsoftJsonPatchInputFormatter>().First();

var app = builder.Build();

// Add Glopal Exception Handler Middleware
var logger = app.Services.GetRequiredService<ILoggerManager>();
app.ConfigureExceptionHandler(logger);
if (app.Environment.IsProduction())
    app.UseHsts();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All
});

app.UseIpRateLimiting();

app.UseCors("CorsPolicy");

app.UseResponseCaching();
app.UseHttpCacheHeaders();

app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(s =>
{
    s.SwaggerEndpoint("/swagger/v1/swagger.json", "Ultimat Asp.net Api v1");
    s.SwaggerEndpoint("/swagger/v2/swagger.json", "Ultimat Asp.net API v2");
});
app.UseReDoc(c =>
{
    c.RoutePrefix = "docs";
    c.SpecUrl = "/swagger/v1/swagger.json";
    c.DocumentTitle = "CompanyEmployees ReDoc UI";
});
app.Map("/shaban", builder =>
{
    builder.Use(async (context, next) =>
    {
        Console.WriteLine("Hello I`m Map Before Run The Next Delegate");
        await next.Invoke();
        Console.WriteLine("Hello I`m Map After Run The Next Delegate");

    });

    builder.Run(async context =>
    {
        Console.WriteLine("Hello I`m Map From Run Method");
        await context.Response.WriteAsync("Hello I`m Map");
    });
});

app.MapWhen(context => context.Request.Query.ContainsKey("shaban"), builder =>
{
    builder.Use(async (context, next) =>
    {
        Console.WriteLine("Hello I`m MapWhen Before Run The Next Delegate");
        await next.Invoke();
        Console.WriteLine("Hello I`m MapWhen After Run The Next Delegate");

    });

    builder.Run(async context =>
    {
        Console.WriteLine("Hello I`m MapWhen From Run Method");
        await context.Response.WriteAsync("Hello I`m MapWhen");
    });
});




app.MapControllers();

app.Run();
