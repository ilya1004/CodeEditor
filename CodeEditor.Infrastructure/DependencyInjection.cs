using CodeEditor.Core.Abstractions.Data;
using CodeEditor.Infrastructure.Data;
using CodeEditor.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CodeEditor.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite("Data Source=..\\code-editor.db"));

        services.AddTransient<IUnitOfWork, AppUnitOfWork>();
        
        return services;
    }
}