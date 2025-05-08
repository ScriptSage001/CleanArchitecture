using System.Reflection;
using API;
using API.Extensions;
using Application;
using Asp.Versioning;
using Asp.Versioning.Builder;
using CorrelationId;
using Infrastructure;
using Persistence;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfig) 
    => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services
            .AddPresentation()
            .AddPersistence(builder.Configuration)
            .AddInfrastructure(builder.Configuration)
            .AddApplication(builder.Configuration);

builder.Services.AddSwaggerGenWithAuth();

builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());

var app = builder.Build();

var apiVersionSet = app
                                .NewApiVersionSet()
                                .HasApiVersion(new ApiVersion(1, 0))
                                .ReportApiVersions()
                                .Build();
var versionedGroup = app
                        .MapGroup("api/v{version:apiVersion}")
                        .WithApiVersionSet(apiVersionSet);

app.MapEndpoints(versionedGroup);

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithUi();

    // app.ApplyMigrations();
}

app.UseCorrelationId();
app.UseRequestContextLogging();
app.UseSerilogRequestLogging();
app.UseExceptionHandler(cfg => cfg.UseCorrelationId());
app.UseAuthentication();
app.UseAuthorization();

await app.RunAsync();