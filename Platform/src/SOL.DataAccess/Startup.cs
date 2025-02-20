using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SOL.Abstractions.Domain;
using SOL.Abstractions.Persistence;

namespace SOL.DataAccess;

public static class Startup
{
    public static IServiceCollection AddDataAccess<TDbCtx>(this IServiceCollection services, Action<DbContextOptionsBuilder> dbOptions) where TDbCtx : DbContext
    {        
        var ctxType = typeof(TDbCtx);
        var assembly = ctxType.Assembly;
        
        var aggregateRoots = assembly.GetTypes().Where(t => typeof(AggregateRoot).IsAssignableFrom(t));
        foreach (var type in aggregateRoots)
        {
            var repositoryType = typeof(GenericRepository<,>).MakeGenericType(ctxType, type);
            var repositoryInterface = typeof(IAggregateRepository<>).MakeGenericType(type);
            var readerInterface = typeof(IAggregateReader<>).MakeGenericType(type);

            services
                .AddTransient(readerInterface, repositoryType)
                .AddTransient(repositoryInterface, repositoryType);
        }
        
        var views = assembly.GetTypes().Where(t => t.Name.EndsWith("View") && t.IsClass && !t.IsAbstract);
        foreach (var type in views)
        {
            var queryType = typeof(GenericQuery<,>).MakeGenericType(ctxType, type);
            var queryInterface = typeof(IDomainQuery<>).MakeGenericType(type);
            
            services.AddTransient(queryInterface, queryType);
        }

        services
            .AddDbContextFactory<TDbCtx>(dbOptions)
            .AddDbContext<TDbCtx>(dbOptions)
            .Scan(scanner => scanner.FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo<ICommitInterceptor>())
                .AsImplementedInterfaces()
                .WithTransientLifetime());
        
        return services;
    }
}