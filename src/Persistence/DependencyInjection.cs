using System.Diagnostics;
using System.Reflection;
using Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Abstractions;

namespace Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAppDbContext(configuration);
        services.AddRepositories();
        services.AddServices();

        return services;
    }

    private static void AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>((optionsBuilder) =>
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("Database"),
            b =>
            {
                b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                b.EnableRetryOnFailure();
            }).EnableSensitiveDataLogging().LogTo(message => Debug.WriteLine(message));
        });
    }

    private static void AddRepositories(this IServiceCollection services)
    {
        var implementations = AssemblyReference
                                                .Assembly    
                                                .GetTypes()
                                                .Where(type => 
                                                    type is { IsClass: true, IsAbstract: false } && 
                                                    type.BaseType != null &&
                                                    type.GetInterfaces()
                                                        .Any(i =>
                                                            i.IsGenericType &&
                                                            i.GetGenericTypeDefinition() == typeof(IRepository<>)));

        foreach (var implementation in implementations)
        {
            var serviceTypes = implementation
                                                    .GetInterfaces()
                                                    .Where(i =>
                                                        i.IsGenericType &&
                                                        i.GetGenericTypeDefinition() == typeof(IRepository<>));
            foreach (var serviceType in serviceTypes)
            {
                services.AddScoped(serviceType, implementation);
            }
        }
    }
    
    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}