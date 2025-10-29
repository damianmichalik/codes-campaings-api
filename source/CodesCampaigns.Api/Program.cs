using CodesCampaigns.Api.Authentication;
using CodesCampaigns.Api.Exceptions;
using CodesCampaigns.Application;
using CodesCampaigns.Infrastructure;
using Hangfire;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails(options =>
    options.CustomizeProblemDetails = context =>
        context.ProblemDetails.Extensions.Add("requestId", context.HttpContext.TraceIdentifier));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API Key needed to access the endpoints. Example: X-API-KEY: {key}",
        In = ParameterLocation.Header,
        Name = "X-API-KEY",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                },
                In = ParameterLocation.Header,
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.Configure<AuthenticationSettings>(builder.Configuration.GetSection("Authentication"));

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard();
app.UseHttpsRedirection();
app.UseExceptionHandler();

app.MapControllers();

await app.RunAsync();

#pragma warning disable S1118
#pragma warning disable CA1515
public partial class Program { }
#pragma warning restore S1118
#pragma warning restore CA1515
