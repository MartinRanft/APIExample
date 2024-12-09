using System.Reflection;

using ByteWizardApi.ApiAuth;
using ByteWizardApi.BackgroundService;
using ByteWizardApi.Interfaces.DB;
using ByteWizardApi.Services.DB;

using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IDatabaseService, DatabaseService>();

//Wireguard watche
builder.Services.AddHostedService<WireGuardMonitoringService>();

//CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    builder.WithOrigins("https://discord.bytewizards.de")
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials());
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API v1", Version = "v1" });
    c.SwaggerDoc("internal", new OpenApiInfo { Title = "Internal API", Version = "internal" });
    c.SwaggerDoc("DCWebsite", new OpenApiInfo { Title = "Discord Website", Version = "DCWebsite" });

    c.OperationFilter<AddApiKeyHeaderOperationFilter>();

    // Group Configuration
    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        if(docName == "v1")
        {
            // v1 Tab Configuration
            return !apiDesc.TryGetMethodInfo(out MethodInfo? methodInfo) ||
                   methodInfo!.DeclaringType!.Namespace!.StartsWith("ByteWizardApi.Controllers.Public");
        }
        if(docName == "internal")
        {
            // internal Tab Configuration
            return apiDesc.TryGetMethodInfo(out MethodInfo? methodInfo) &&
                   methodInfo!.DeclaringType!.Namespace!.StartsWith("ByteWizardApi.Controllers.Intern");
        }
        if(docName == "DCWebsite")
        {
            // Discord Website Tab Configuration
            return apiDesc.TryGetMethodInfo(out MethodInfo? methodInfo) &&
                   methodInfo!.DeclaringType!.Namespace!.StartsWith("ByteWizardApi.Controllers.DCWebsite");
        }
        return false;
    });
});

builder.Services.AddSwaggerGenNewtonsoftSupport();
builder.Services.AddApplicationInsightsTelemetry();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.SwaggerEndpoint("/swagger/internal/swagger.json", "Intern");
    options.SwaggerEndpoint("/swagger/DCWebsite/swagger.json", "DCWebsite");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

app.Run();